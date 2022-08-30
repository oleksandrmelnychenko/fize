using FizeRegistration.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using FizeRegistration.Client.Helpers;
using Blazored.LocalStorage;
using FizeRegistration.Client.Services.HttpService.Contracts;
using FizeRegistration.Client.Services.HttpService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddHttpClient<IFizeHttpService, FizeHttpService>(client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});

builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();



await builder.Build().RunAsync();
