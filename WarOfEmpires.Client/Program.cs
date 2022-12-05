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

builder.Services.AddSingleton<JwtSecurityTokenHandler>();
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddSingleton<AccessControlService, AccessControlService>();
builder.Services.AddSingleton<AuthenticationStateProvider, AccessControlService>(provider => provider.GetRequiredService<AccessControlService>());
builder.Services.AddSingleton<IAccessControlService, AccessControlService>(provider => provider.GetRequiredService<AccessControlService>());
builder.Services.AddSingleton<IRoutingService, RoutingService>();
builder.Services.AddSingleton<IPasswordStrengthCalculator, PasswordStrengthCalculator>();

builder.Services.AddHttpClient(Constants.UnauthenticatedClient, client => client.BaseAddress = new Uri(apiSettings.BaseUrl));
builder.Services.AddHttpClient(Constants.AuthenticatedClient, async (provider, client) => {
    client.BaseAddress = new Uri(apiSettings.BaseUrl);
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.AuthenticationScheme, await provider.GetRequiredService<IAccessControlService>().GetAccessToken());
});

builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddAuthorizationCore();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

await builder.Build().RunAsync();
