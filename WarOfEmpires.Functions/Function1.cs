using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System;
using WarOfEmpires.CommandHandlers.Events;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Repositories.Events;
using WarOfEmpires.Utilities.Events;

namespace WarOfEmpires.Functions {
    public static class Function1 {
        [FunctionName("Function1")]
        public static void Run([TimerTrigger("0 * */5 * * *")]TimerInfo myTimer, TraceWriter log) {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

            var command = new RunScheduledTasksCommand();
            var handler = new RunScheduledTasksCommandHandler(new ScheduledTaskRepository(), new EventService());
        }
    }
}