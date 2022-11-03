using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System;
using WarOfEmpires.Client;
using WarOfEmpires.Client.Configuration;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var apiSettings = builder.Configuration.GetSection(nameof(ApiSettings)).Get<ApiSettings>();

builder.Services.AddSingleton(apiSettings);
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiSettings.BaseUrl) });

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

await builder.Build().RunAsync();
