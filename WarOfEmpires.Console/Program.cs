using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Utilities.Configuration;
using WarOfEmpires.Utilities.DependencyInjection;

namespace WarOfEmpires.Console {
    public static class Program {
        static void Main() {
            // TODO convert to scheduled?

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .AddUserSecrets("c61bb83f-efb6-4b20-a8d9-eca92057923a");
            var configuration = builder.Build();            
            var serviceProvider = new ServiceCollection()
                .AddServices(typeof(Program).Assembly)
                .AddSingleton(configuration.GetSection(AppSettings.Key).Get<AppSettings>())
                .BuildServiceProvider(new ServiceProviderOptions {
                    ValidateOnBuild = true,
                    ValidateScopes = true
                });

            using (var scope = serviceProvider.CreateScope()) {
                var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<RunScheduledTasksCommand>>();

                handler.Execute(new RunScheduledTasksCommand());
            }
        }
    }
}