using Backend;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Añadir DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(
		builder.Configuration.GetConnectionString("DefaultConnection")
		)
);

// Añadir repositorios.
builder.Services.AddScoped<IProductRepository, ProductRepository>();
// Añadir services.
builder.Services.AddScoped<IProductService, ProductService>();
// Añadir controllers
builder.Services.AddControllers().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
