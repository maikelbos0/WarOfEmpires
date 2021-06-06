using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using WarOfEmpires.Commands.Events;

namespace WarOfEmpires.Services {
    public sealed class ScheduledTaskRunnerService : BackgroundService {
        private IServiceScopeFactory _serviceScopeFactory;

        public ScheduledTaskRunnerService(IServiceScopeFactory serviceScopeFactory) {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken) {
            Task.Run(() => RunScheduledTasks(cancellationToken));

            return Task.CompletedTask;
        }

        private async Task RunScheduledTasks(CancellationToken cancellationToken) {
            while (!cancellationToken.IsCancellationRequested) {
                using (var scope = _serviceScopeFactory.CreateScope()) {
                    scope.ServiceProvider.GetRequiredService<IMessageService>().Dispatch(new RunScheduledTasksCommand());
                }

                await Task.Delay(15 * 1000);
            }
        }
    }
}
