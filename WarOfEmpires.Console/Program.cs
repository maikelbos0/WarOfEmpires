using Microsoft.Extensions.DependencyInjection;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Utilities.Reflection;

namespace WarOfEmpires.Console {
    public static class Program {
        static void Main() {
            var classFinder = new ClassFinder();
            var serviceCollection = new ServiceCollection();

            // TODO check if we need classfinder still
            foreach (var assembly in classFinder.FindAllAssemblies()) {
                // TODO move to more centralized, refactor to maybe not use classfinder
                // TODO add decorations
                serviceCollection.AddAttributeServices(assembly);
            }

            var serviceProvider = serviceCollection.BuildServiceProvider(new ServiceProviderOptions {
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