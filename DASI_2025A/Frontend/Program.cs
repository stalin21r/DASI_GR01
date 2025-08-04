using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Frontend;
using CurrieTechnologies.Razor.SweetAlert2;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Servicios comunes
builder.Services.AddSweetAlert2();
builder.Services.AddBlazoredLocalStorage();

// Configuración de autenticación y autorización
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthStateProvider>());

// HttpClient público (sin token) - debe estar antes de usarlo en otros servicios
builder.Services.AddScoped(sp => new HttpClient
{
  BaseAddress = new Uri("https://localhost:7055")
  //BaseAddress = new Uri("http://localhost:5067/")
});

// HttpClient con token
builder.Services.AddTransient<AuthorizationMessageHandler>();
builder.Services.AddHttpClient("AuthorizedClient", client =>
{
  client.BaseAddress = new Uri("https://localhost:7055");
  //client.BaseAddress = new Uri("http://localhost:5067/");
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
builder.Services.AddScoped<IOccupationService>(sp =>
{
  var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
  var httpClient = clientFactory.CreateClient("AuthorizedClient");
  return new OccupationService(httpClient);
});
builder.Services.AddScoped<IBranchService>(sp =>
{
  var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
  var httpClient = clientFactory.CreateClient("AuthorizedClient");
  return new BranchService(httpClient);
});
builder.Services.AddScoped<IOrderService>(sp =>
{
  var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
  var httpClient = clientFactory.CreateClient("AuthorizedClient");
  return new OrderService(httpClient);
});

await builder.Build().RunAsync();