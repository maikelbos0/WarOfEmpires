using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Siege;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class UntrainWorkersCommandHandler : ICommandHandler<UntrainWorkersCommand> {
        public PlayerRepository _repository;

        public UntrainWorkersCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<UntrainWorkersCommand> Execute(UntrainWorkersCommand command) {
            var result = new CommandResult<UntrainWorkersCommand>();
            var player = _repository.Get(command.Email);
            var workers = new List<WorkerInfo>();
            int value = 0;

            // Farmers
            if (!string.IsNullOrEmpty(command.Farmers) && !int.TryParse(command.Farmers, out value) || value < 0) {
                result.AddError(c => c.Farmers, "Farmers must be a valid number");
            }
            else if (value > player.GetWorkerCount(WorkerType.Farmer)) {
                result.AddError(c => c.Farmers, "You don't have that many farmers to untrain");
            }
            else if (value > 0) {
                workers.Add(new WorkerInfo(WorkerType.Farmer, value));
            }

            // Wood workers
            value = 0;
            if (!string.IsNullOrEmpty(command.WoodWorkers) && !int.TryParse(command.WoodWorkers, out value) || value < 0) {
                result.AddError(c => c.WoodWorkers, "Wood workers must be a valid number");
            }
            else if (value > player.GetWorkerCount(WorkerType.WoodWorker)) {
                result.AddError(c => c.WoodWorkers, "You don't have that many wood workers to untrain");
            }
            else if (value > 0) {
                workers.Add(new WorkerInfo(WorkerType.WoodWorker, value));
            }

            // Stone masons
            value = 0;
            if (!string.IsNullOrEmpty(command.StoneMasons) && !int.TryParse(command.StoneMasons, out value) || value < 0) {
                result.AddError(c => c.StoneMasons, "Stone masons must be a valid number");
            }
            else if (value > player.GetWorkerCount(WorkerType.StoneMason)) {
                result.AddError(c => c.StoneMasons, "You don't have that many stone masons to untrain");
            }
            else if (value > 0) {
                workers.Add(new WorkerInfo(WorkerType.StoneMason, value));
            }

            // Ore miners
            value = 0;
            if (!string.IsNullOrEmpty(command.OreMiners) && !int.TryParse(command.OreMiners, out value) || value < 0) {
                result.AddError(c => c.OreMiners, "Ore miners must be a valid number");
            }
            else if (value > player.GetWorkerCount(WorkerType.OreMiner)) {
                result.AddError(c => c.OreMiners, "You don't have that many ore miners to untrain");
            }
            else if (value > 0) {
                workers.Add(new WorkerInfo(WorkerType.OreMiner, value));
            }

            // Siege engineers
            value = 0;
            if (!string.IsNullOrEmpty(command.SiegeEngineers) && !int.TryParse(command.SiegeEngineers, out value) || value < 0) {
                result.AddError(c => c.SiegeEngineers, "Siege engineers must be a valid number");
            }
            else if (value > player.GetWorkerCount(WorkerType.SiegeEngineer)) {
                result.AddError(c => c.SiegeEngineers, "You don't have that many siege engineers to untrain");
            }
            else if ((player.GetWorkerCount(WorkerType.SiegeEngineer) - value) * player.GetBuildingBonus(BuildingType.SiegeFactory) < player.SiegeWeapons.Sum(s => s.Count * SiegeWeaponDefinitionFactory.Get(s.Type).Maintenance)) {
                result.AddError(c => c.SiegeEngineers, "Your siege engineers are maintaining too many siege weapons for that many to be untrained");
            }
            else if (value > 0) {
                workers.Add(new WorkerInfo(WorkerType.SiegeEngineer, value));
            }

            if (result.Success) {
                foreach (var workerInfo in workers) {
                    player.UntrainWorkers(workerInfo.Type, workerInfo.Count);
                }

                _repository.Update();
            }

            return result;
        }
    }
}