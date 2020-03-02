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
        public PlayerRepository _repository;

        public TrainWorkersCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<TrainWorkersCommand> Execute(TrainWorkersCommand command) {
            var result = new CommandResult<TrainWorkersCommand>();
            var player = _repository.Get(command.Email);
            var workers = new List<WorkerInfo>();
            int value = 0;

            // Farmers
            if (!string.IsNullOrEmpty(command.Farmers) && !int.TryParse(command.Farmers, out value) || value < 0) {
                result.AddError(c => c.Farmers, "Farmers must be a valid number");
            }
            else if (value > 0) {
                workers.Add(new WorkerInfo(WorkerType.Farmer, value));
            }

            // Wood workers
            value = 0;
            if (!string.IsNullOrEmpty(command.WoodWorkers) && !int.TryParse(command.WoodWorkers, out value) || value < 0) {
                result.AddError(c => c.WoodWorkers, "Wood workers must be a valid number");
            }
            else if (value > 0) {
                workers.Add(new WorkerInfo(WorkerType.WoodWorker, value));
            }

            // Stone masons
            value = 0;
            if (!string.IsNullOrEmpty(command.StoneMasons) && !int.TryParse(command.StoneMasons, out value) || value < 0) {
                result.AddError(c => c.StoneMasons, "Stone masons must be a valid number");
            }
            else if (value > 0) {
                workers.Add(new WorkerInfo(WorkerType.StoneMason, value));
            }

            // Ore miners
            value = 0;
            if (!string.IsNullOrEmpty(command.OreMiners) && !int.TryParse(command.OreMiners, out value) || value < 0) {
                result.AddError(c => c.OreMiners, "Ore miners must be a valid number");
            }
            else if (value > 0) {
                workers.Add(new WorkerInfo(WorkerType.OreMiner, value));
            }

            // Siege engineers
            value = 0;
            if (!string.IsNullOrEmpty(command.SiegeEngineers) && !int.TryParse(command.SiegeEngineers, out value) || value < 0) {
                result.AddError(c => c.SiegeEngineers, "Siege engineers must be a valid number");
            }
            else if (value > 0) {
                workers.Add(new WorkerInfo(WorkerType.SiegeEngineer, value));
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

                _repository.Update();
            }

            return result;
        }
    }
}