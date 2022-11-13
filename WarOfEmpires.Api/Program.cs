using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using VDT.Core.DependencyInjection;
using VDT.Core.DependencyInjection.Attributes;
using VDT.Core.DependencyInjection.Decorators;
using WarOfEmpires.Api;
using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Database.Auditing;
using WarOfEmpires.QueryHandlers;
using WarOfEmpires.Utilities.Configuration;
using WarOfEmpires.Utilities.Events;
using WarOfEmpires.Utilities.Mail;

var builder = WebApplication.CreateBuilder(args);
var issuerSigningKey = new SymmetricSecurityKey(RandomNumberGenerator.GetBytes(32));
var clientSettings = builder.Configuration.GetSection(nameof(ClientSettings)).Get<ClientSettings>();

builder.Services.AddSingleton<SecurityKey>(issuerSigningKey);
builder.Services.AddSingleton(clientSettings);
builder.Services.AddSingleton(builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>());

// TODO swagger config
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddServices(options => options
    .AddAssemblies(typeof(Program).Assembly, nameof(WarOfEmpires))
    .AddServiceTypeProvider(DefaultServiceTypeProviders.InterfaceByName)
    .AddServiceTypeProvider(DefaultServiceTypeProviders.CreateGenericInterfaceTypeProvider(typeof(IEventHandler<>)))
    .AddServiceTypeProvider(DefaultServiceTypeProviders.CreateGenericInterfaceTypeProvider(typeof(IMailTemplate<>)))
    .AddAttributeServiceTypeProviders()
    .UseDefaultServiceLifetime(ServiceLifetime.Transient)
);
builder.Services.AddServices(options => options
    .AddAssemblies(typeof(Program).Assembly, nameof(WarOfEmpires))
    .AddServiceTypeProvider(DefaultServiceTypeProviders.CreateGenericInterfaceTypeProvider(typeof(ICommandHandler<>)))
    .AddServiceTypeProvider(DefaultServiceTypeProviders.CreateGenericInterfaceTypeProvider(typeof(IQueryHandler<,>)))
    .UseDefaultServiceLifetime(ServiceLifetime.Transient)
    .UseDecoratorServiceRegistrar(decoratorOptions => decoratorOptions.AddDecorator<IAuditDecorator>())
);

builder.Services.AddCors(options => options.AddDefaultPolicy(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins(clientSettings.BaseUrl)));

// TODO test auth, probably move token generation to API
builder.Services.AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => {
        options.TokenValidationParameters = new TokenValidationParameters() {
            RequireAudience = true,
            ValidateAudience = true,
            ValidAudience = clientSettings.TokenAudience,
            ValidateIssuer = true,
            ValidIssuer = clientSettings.TokenIssuer,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = issuerSigningKey,
            RequireExpirationTime = true,
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization(builder => {
    builder.DefaultPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .RequireClaim(JwtRegisteredClaimNames.Aud, clientSettings.TokenAudience)
        .RequireClaim(JwtRegisteredClaimNames.Iss, clientSettings.TokenIssuer)
        .Build();

    builder.AddPolicy(Policies.Administrator, new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .RequireClaim(JwtRegisteredClaimNames.Aud, clientSettings.TokenAudience)
        .RequireClaim(JwtRegisteredClaimNames.Iss, clientSettings.TokenIssuer)
        .RequireRole(Policies.Administrator)
        .Build());
});

builder.Services.AddControllers();

var app = builder.Build();

// TODO swagger config
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
