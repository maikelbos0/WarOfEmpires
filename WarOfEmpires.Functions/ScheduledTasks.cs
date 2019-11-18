using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using Unity;
using WarOfEmpires.CommandHandlers.Events;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Functions.Container;

namespace WarOfEmpires.Functions {
    public static class ScheduledTasks {
        [FunctionName("ScheduledTasks")]
        public static void Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger logger) {
            logger.LogInformation($"ScheduledTasks Timer trigger function executed at: {DateTime.UtcNow}");

            var handler = UnityConfig.Container.Resolve<RunScheduledTasksCommandHandler>();

            handler.Execute(new RunScheduledTasksCommand());
        }
    }
}