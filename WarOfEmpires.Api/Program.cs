using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VDT.Core.DependencyInjection;
using VDT.Core.DependencyInjection.Attributes;
using VDT.Core.DependencyInjection.Decorators;
using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Database.Auditing;
using WarOfEmpires.QueryHandlers;
using WarOfEmpires.Utilities.Configuration;
using WarOfEmpires.Utilities.Events;
using WarOfEmpires.Utilities.Mail;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

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

builder.Services.AddSingleton(builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>());

var app = builder.Build();

// TODO swagger config
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// TODO auth
app.UseAuthorization();

app.MapControllers();

app.Run();
