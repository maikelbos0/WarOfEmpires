using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using WarOfEmpires.Client;
using WarOfEmpires.Client.Configuration;
using WarOfEmpires.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var apiSettings = builder.Configuration.GetSection(nameof(ApiSettings)).Get<ApiSettings>();

builder.Services.AddSingleton(apiSettings);
builder.Services.AddScoped<HttpMessageHandler, HttpClientHandler>();
builder.Services.AddScoped<TokenHandler>();
builder.Services.AddScoped(provider => new HttpClient(provider.GetRequiredService<TokenHandler>()) { BaseAddress = new Uri(apiSettings.BaseUrl) });
builder.Services.AddScoped<RoutingService>();
builder.Services.AddScoped<PasswordStrengthCalculator>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<JwtSecurityTokenHandler>();
builder.Services.AddScoped<AccessControlProvider>();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

await builder.Build().RunAsync();
