using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Linq;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class TrainWorkersCommandHandler : ICommandHandler<TrainWorkersCommand> {
        private readonly IPlayerRepository _repository;

        public TrainWorkersCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<TrainWorkersCommand> Execute(TrainWorkersCommand command) {
            var result = new CommandResult<TrainWorkersCommand>();
            var player = _repository.Get(command.Email);
            var workers = new List<WorkerInfo>();

            for (var index = 0; index < command.Workers.Count; index++) {
                var i = index; // Don't use iterator in lambdas
                var type = (WorkerType)Enum.Parse(typeof(WorkerType), command.Workers[i].Type);
                int count = 0;

                if (!string.IsNullOrEmpty(command.Workers[i].Count) && !int.TryParse(command.Workers[i].Count, out count) || count < 0) {
                    result.AddError(c => c.Workers[i].Count, "Invalid number");
                }

                if (result.Success && count > 0) {
                    workers.Add(new WorkerInfo(type, count));
                }
            }

            if (workers.Sum(w => w.Count) > player.Peasants) {
                result.AddError("You don't have that many peasants available to train");
            }

            if (workers.Sum(w => w.Count) > player.GetAvailableHutCapacity()) {
                result.AddError("You don't have enough huts available to train that many workers");
            }

            if (!player.CanAfford(workers.Sum(w => w.Count * WorkerDefinitionFactory.Get(w.Type).Cost))) {
                result.AddError("You don't have enough gold to train these peasants");
            }

            if (result.Success) {
                foreach (var workerInfo in workers) {
                    player.TrainWorkers(workerInfo.Type, workerInfo.Count);
                }

                _repository.SaveChanges();
            }

            return result;
        }
    }
}