using Unity;
using WarOfEmpires.CommandHandlers.Events;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Reflection;

namespace WarOfEmpires.Console {
    public static class Program {
        static void Main() {
            var containerService = new ContainerService(new ClassFinder());

            using (var container = containerService.GetContainer()) {
                var handler = container.Resolve<RunScheduledTasksCommandHandler>();

                handler.Execute(new RunScheduledTasksCommand());
            }
        }
    }
}