using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        private IEnumerable<WorkerInfo> ParseWorkers(UntrainWorkersCommand command,
                                                     CommandResult<UntrainWorkersCommand> result,
                                                     WorkerType type,
                                                     Expression<Func<UntrainWorkersCommand, object>> workerFunc,
                                                     int maximumWorkers) {

            var commandWorkers = (string)workerFunc.Compile().Invoke(command);
            int workers = 0;

            if (!string.IsNullOrEmpty(commandWorkers) && !int.TryParse(commandWorkers, out workers) || workers < 0) {
                result.AddError(workerFunc, $"{type.ToString()} must be a valid number");
            }
            else if (workers > maximumWorkers) {
                result.AddError(workerFunc, $"You don't have that many {type.ToString().ToLower()} to untrain");
            }

            if (result.Success && workers > 0) {
                yield return new WorkerInfo(type, workers);
            }
        }

        public CommandResult<UntrainWorkersCommand> Execute(UntrainWorkersCommand command) {
            var result = new CommandResult<UntrainWorkersCommand>();
            var player = _repository.Get(command.Email);
            var workers = new List<WorkerInfo>();

            workers.AddRange(ParseWorkers(command, result, WorkerType.Farmer, c => c.Farmers, player.GetWorkerCount(WorkerType.Farmer)));
            workers.AddRange(ParseWorkers(command, result, WorkerType.WoodWorker, c => c.WoodWorkers, player.GetWorkerCount(WorkerType.WoodWorker)));
            workers.AddRange(ParseWorkers(command, result, WorkerType.StoneMason, c => c.StoneMasons, player.GetWorkerCount(WorkerType.StoneMason)));
            workers.AddRange(ParseWorkers(command, result, WorkerType.OreMiner, c => c.OreMiners, player.GetWorkerCount(WorkerType.OreMiner)));
            workers.AddRange(ParseWorkers(command, result, WorkerType.SiegeEngineer, c => c.SiegeEngineers, player.GetWorkerCount(WorkerType.SiegeEngineer)));

            var siegeEngineers = workers.SingleOrDefault(w => w.Type == WorkerType.SiegeEngineer)?.Count ?? 0;

            if (siegeEngineers > 0 && (player.GetWorkerCount(WorkerType.SiegeEngineer) - siegeEngineers) * player.GetBuildingBonus(BuildingType.SiegeFactory) < player.SiegeWeapons.Sum(s => s.Count * SiegeWeaponDefinitionFactory.Get(s.Type).Maintenance)) {
                result.AddError(c => c.SiegeEngineers, "Your siege engineers are maintaining too many siege weapons for that many to be untrained");
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