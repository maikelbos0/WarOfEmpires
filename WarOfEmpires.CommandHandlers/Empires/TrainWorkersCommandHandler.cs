using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        private IEnumerable<WorkerInfo> ParseWorkers(TrainWorkersCommand command,
                                                     CommandResult<TrainWorkersCommand> result,
                                                     WorkerType type,
                                                     Expression<Func<TrainWorkersCommand, object>> workerFunc) {

            var commandWorkers = (string)workerFunc.Compile().Invoke(command);
            int workers = 0;

            if (!string.IsNullOrEmpty(commandWorkers) && !int.TryParse(commandWorkers, out workers) || workers < 0) {
                result.AddError(workerFunc, $"{type.ToString()} must be a valid number");
            }

            if (result.Success && workers > 0) {
                yield return new WorkerInfo(type, workers);
            }
        }

        public CommandResult<TrainWorkersCommand> Execute(TrainWorkersCommand command) {
            var result = new CommandResult<TrainWorkersCommand>();
            var player = _repository.Get(command.Email);
            var workers = new List<WorkerInfo>();

            workers.AddRange(ParseWorkers(command, result, WorkerType.Farmer, c => c.Farmers));
            workers.AddRange(ParseWorkers(command, result, WorkerType.WoodWorker, c => c.WoodWorkers));
            workers.AddRange(ParseWorkers(command, result, WorkerType.StoneMason, c => c.StoneMasons));
            workers.AddRange(ParseWorkers(command, result, WorkerType.OreMiner, c => c.OreMiners));
            workers.AddRange(ParseWorkers(command, result, WorkerType.SiegeEngineer, c => c.SiegeEngineers));

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