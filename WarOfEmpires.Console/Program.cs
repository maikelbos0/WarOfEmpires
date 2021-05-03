using Microsoft.Extensions.DependencyInjection;
using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Utilities.DependencyInjection;

namespace WarOfEmpires.Console {
    public static class Program {
        static void Main() {
            // TODO add configuration with user secret for SendGrid API key
            var serviceProvider = new ServiceCollection()
                .AddServices(typeof(Program).Assembly)
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