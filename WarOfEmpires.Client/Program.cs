using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using WarOfEmpires.Client;
using WarOfEmpires.Client.Configuration;
using WarOfEmpires.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var apiSettings = builder.Configuration.GetSection(nameof(ApiSettings)).Get<ApiSettings>();

builder.Services.AddSingleton(apiSettings);

builder.Services.AddScoped<JwtSecurityTokenHandler>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<AccessControlService, AccessControlService>();
builder.Services.AddScoped<AuthenticationStateProvider, AccessControlService>(provider => provider.GetRequiredService<AccessControlService>());
builder.Services.AddScoped<IAccessControlService, AccessControlService>(provider => provider.GetRequiredService<AccessControlService>());
builder.Services.AddScoped<IRoutingService, RoutingService>();
builder.Services.AddScoped<IPasswordStrengthCalculator, PasswordStrengthCalculator>();

builder.Services.AddHttpClient(Constants.UnauthenticatedClient, client => client.BaseAddress = new Uri(apiSettings.BaseUrl));
builder.Services.AddHttpClient(Constants.AuthenticatedClient, async (provider, client) => {
    client.BaseAddress = new Uri(apiSettings.BaseUrl);
    // TODO fix scope?
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.AuthenticationScheme, await provider.GetRequiredService<IAccessControlService>().GetAccessToken());
});

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

await builder.Build().RunAsync();
