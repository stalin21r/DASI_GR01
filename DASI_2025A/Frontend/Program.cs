using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Frontend;
using CurrieTechnologies.Razor.SweetAlert2;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Servicios comunes
builder.Services.AddSweetAlert2();
builder.Services.AddBlazoredLocalStorage();

// HttpClient público (sin token) - debe estar antes de usarlo en otros servicios
builder.Services.AddScoped(sp => new HttpClient
{
  BaseAddress = new Uri("https://localhost:7055")
});

// HttpClient con token
builder.Services.AddTransient<AuthorizationMessageHandler>();
builder.Services.AddHttpClient("AuthorizedClient", client =>
{
  client.BaseAddress = new Uri("https://localhost:7055");
}).AddHttpMessageHandler<AuthorizationMessageHandler>();

// Servicios personalizados
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService>(sp =>
{
  var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
  var httpClient = clientFactory.CreateClient("AuthorizedClient");
  return new ProductService(httpClient);
});
builder.Services.AddScoped<IUsersService>(sp =>
{
  var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
  var httpClient = clientFactory.CreateClient("AuthorizedClient");
  return new UsersService(httpClient);
});

await builder.Build().RunAsync();



