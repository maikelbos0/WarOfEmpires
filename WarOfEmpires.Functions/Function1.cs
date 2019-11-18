using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace WarOfEmpires.Functions {
    public static class Function1 {
        [FunctionName("Function1")]
        public static void Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger logger) {
            logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}