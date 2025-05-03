using Backend;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Backend.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Backend.Services.Interfaces;
using Backend.Services;


var builder = WebApplication.CreateBuilder(args);

// Añadir DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(
		builder.Configuration.GetConnectionString("DefaultConnection")
		)
);

// ---------- Identity ----------
builder.Services
    .AddIdentityCore<ApplicationUser>(opt =>
    {
       
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// ---------- JWT ----------
var jwtCfg = builder.Configuration.GetSection("Jwt"); // Configuracion JWT de appsettings.json
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    // Configuracion del middleware JWT Bearer
.AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtCfg["Issuer"],
        ValidAudience = jwtCfg["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtCfg["Key"]!))
    };
});


// ---------- Autorizacion ----------
builder.Services.AddAuthorization();


// Añadir repositorios.
builder.Services.AddScoped<IProductRepository, ProductRepository>();
// Añadir services.
builder.Services.AddScoped<IProductService, ProductService>();
// Añadir controllers
builder.Services.AddControllers().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// ---------- servicio de autenticación ----------
builder.Services.AddScoped<IAuthService, AuthService>();

// Configure CORS
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowBlazorApp", builder =>
	{
		builder.WithOrigins("https://localhost:7206")
				 .AllowAnyMethod()
				 .AllowAnyHeader()
				 .AllowCredentials();
	});
});


// -------------------- Swagger --------------------
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Definir el esquema «Bearer»
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Escribe: **Bearer &lt;token&gt;**"
    });

    // Hacer que Swagger exija el token para los endpoints protegidos
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id   = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
});
// --------------------------------------------------



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Usar CORS - importante colocarlo antes de UseAuthorization y UseEndpoints
app.UseCors("AllowBlazorApp");


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Anade los roles una vez al iniciar la aplicacion
async Task SeedRolesAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    
    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // Crear los roles que falten
    string[] roles = { "Superadmin", "Admin", "User" };
    foreach (var role in roles)
        if (!await roleMgr.RoleExistsAsync(role))
            await roleMgr.CreateAsync(new IdentityRole(role));

    // Datos del super‑admin inicial
    const string superEmail = "super@local";   // cambiar en prod
    const string superPassword = "Password123@";   // moverlo en prod

    var super = await userMgr.FindByEmailAsync(superEmail);
    if (super is null)
    {
        super = new ApplicationUser
        {
            UserName = superEmail,
            Email = superEmail,
            FirstName = "Super",
            LastName = "Admin"
        };

        var create = await userMgr.CreateAsync(super, superPassword);
        if (!create.Succeeded)
            throw new Exception("Error creando Superadmin: " +
                string.Join("; ", create.Errors.Select(e => e.Description)));
    }

    // Asegurar rol Superadmin
    if (!await userMgr.IsInRoleAsync(super, "Superadmin"))
        await userMgr.AddToRoleAsync(super, "Superadmin");
}
await SeedRolesAsync(app.Services);

app.Run();
