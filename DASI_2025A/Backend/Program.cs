using Backend;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.DataProtection;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// ===============================
// CONFIGURACIÓN DE SERVICIOS
// ===============================

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.None);

builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireNonAlphanumeric = true;
	options.Password.RequireUppercase = true;
	options.Password.RequiredLength = 8;
	options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Repositorios
builder.Services.AddHttpClient();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductLoggerRepository, ProductLoggerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOccupationRepository, OccupationRepository>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
// Servicios
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductLoggerService, ProductLoggerService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IImgurService, ImgurService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOccupationService, OccupationService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IMailkitService, MailkitService>();

// Controllers
builder.Services.AddControllers().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	// Configuración para habilitar autorización JWT en Swagger
	c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
	{
		Title = "DASI API",
		Version = "v1",
		Description = "API para el proyecto DASI"
	});

	// Definir el esquema de seguridad
	c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
	{
		Name = "Authorization",
		Description = "Ingrese el token JWT:",
		In = Microsoft.OpenApi.Models.ParameterLocation.Header,
		Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});

	// Aplicar el esquema de seguridad globalmente
	c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
	{
		{
			new Microsoft.OpenApi.Models.OpenApiSecurityScheme
			{
				Reference = new Microsoft.OpenApi.Models.OpenApiReference
				{
					Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});
});

// Crear directorio para claves si no existe
var keysDirectory = new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "DataProtection-Keys"));
if (!keysDirectory.Exists)
{
	keysDirectory.Create();
}

// Configuración cross-platform de Data Protection
var dataProtectionBuilder = builder.Services.AddDataProtection()
		.PersistKeysToFileSystem(keysDirectory)
		.SetApplicationName("DASI_App")
		.SetDefaultKeyLifetime(TimeSpan.FromDays(90));

// Solo usar DPAPI en Windows
if (OperatingSystem.IsWindows())
{
	dataProtectionBuilder.ProtectKeysWithDpapi(protectToLocalMachine: true);
}
else
{
	// Para Linux/macOS usar encriptación con certificado o clave
	// En desarrollo, esto es suficiente (las claves están en el filesystem protegido)
	builder.Logging.AddConsole();
	var logger = LoggerFactory.Create(config => config.AddConsole()).CreateLogger("DataProtection");
	logger.LogInformation("DPAPI no disponible en esta plataforma. Las claves se almacenan sin encriptación adicional.");
}

// ===============================
// CONFIGURACIÓN DE RATE LIMITING (Previene ataques DDoS y de fuerza bruta)
// ===============================
builder.Services.AddRateLimiter(options =>
{
	// Límite general de API
	options.AddFixedWindowLimiter("ApiLimit", configure =>
	{
		configure.Window = TimeSpan.FromMinutes(1);
		configure.PermitLimit = 100; // 100 requests por minuto
		configure.QueueLimit = 10;
		configure.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
	});

	// Límite estricto para autenticación (previene ataques de fuerza bruta)
	options.AddFixedWindowLimiter("AuthLimit", configure =>
	{
		configure.Window = TimeSpan.FromMinutes(5);
		configure.PermitLimit = 5; // Solo 5 intentos de login cada 5 minutos
		configure.QueueLimit = 0;
	});

	// Configuración global de rechazo
	options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
			RateLimitPartition.GetFixedWindowLimiter(
					partitionKey: context.Request.Headers.Host.ToString(),
					factory: partition => new FixedWindowRateLimiterOptions
					{
						AutoReplenishment = true,
						PermitLimit = 1000, // 1000 requests por minuto por host
						Window = TimeSpan.FromMinutes(1)
					}));
});

// CORS
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowBlazorApp", builder =>
	{
		builder.WithOrigins("https://localhost:7206", "http://localhost:5007")
				 .AllowAnyMethod()
				 .AllowAnyHeader()
				 .AllowCredentials();
	});
});

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.RequireHttpsMetadata = false;
	options.SaveToken = true;
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(key),
		ValidateIssuer = true,
		ValidIssuer = jwtSettings["Issuer"],
		ValidateAudience = true,
		ValidAudience = jwtSettings["Audience"],
		ValidateLifetime = true,
		ClockSkew = TimeSpan.Zero
	};

	options.Events = new JwtBearerEvents
	{
		OnMessageReceived = context =>
		{
			if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
			{
				var token = authHeader.ToString();
				if (!token.StartsWith("Bearer "))
				{
					context.Request.Headers["Authorization"] = "Bearer " + token;
				}
			}
			return Task.CompletedTask;
		},
		OnChallenge = context =>
		{
			context.HandleResponse();

			if (context.Response.HasStarted)
				return Task.CompletedTask;

			context.Response.StatusCode = StatusCodes.Status401Unauthorized;
			context.Response.ContentType = "application/json";

			var mensaje = string.IsNullOrEmpty(context.Request.Headers["Authorization"])
				? "{\"message\":\"No se envió un token\"}"
				: "{\"message\":\"Sesión no válida\"}";

			return context.Response.WriteAsync(mensaje);
		},
		OnAuthenticationFailed = context =>
		{
			if (context.Response.HasStarted)
				return Task.CompletedTask;

			context.NoResult();
			context.Response.StatusCode = StatusCodes.Status401Unauthorized;
			context.Response.ContentType = "application/json";
			return context.Response.WriteAsync("{\"message\":\"Token inválido o expirado\"}");
		}
	};
});

// Authorization Policies
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("SuperadminOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Superadmin"));
	options.AddPolicy("AdminPlus", policy => policy.RequireClaim(ClaimTypes.Role, "Superadmin", "Admin"));
});

// ===============================
// CONFIGURACIÓN DE LA APP
// ===============================

var app = builder.Build();

// Swagger solo en desarrollo
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowBlazorApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Inicialización de usuarios y roles
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	try
	{
		await IdentityInitializer.InitializeAsync(services);
	}
	catch (Exception ex)
	{
		var logger = services.GetRequiredService<ILogger<Program>>();
		logger.LogError(ex, "An error occurred while seeding the DB.");
	}
}

app.Run();
