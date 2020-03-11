using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Formatting;
using WarOfEmpires.Utilities.Linq;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class TrainWorkersCommandHandler : ICommandHandler<TrainWorkersCommand> {
        private readonly PlayerRepository _repository;
        private readonly EnumFormatter _formatter;

        public TrainWorkersCommandHandler(PlayerRepository repository, EnumFormatter formatter) {
            _repository = repository;
            _formatter = formatter;
        }

        private IEnumerable<WorkerInfo> ParseWorkers(TrainWorkersCommand command,
                                                     CommandResult<TrainWorkersCommand> result,
                                                     WorkerType type,
                                                     Expression<Func<TrainWorkersCommand, object>> workerFunc) {

            var commandWorkers = (string)workerFunc.Compile().Invoke(command);
            int workers = 0;

            if (!string.IsNullOrEmpty(commandWorkers) && !int.TryParse(commandWorkers, out workers) || workers < 0) {
                result.AddError(workerFunc, $"{_formatter.ToString(type)} must be a valid number");
            }

            if (result.Success && workers > 0) {
                yield return new WorkerInfo(type, workers);
            }
        }

        public CommandResult<TrainWorkersCommand> Execute(TrainWorkersCommand command) {
            var result = new CommandResult<TrainWorkersCommand>();
            var player = _repository.Get(command.Email);
            var workers = new List<WorkerInfo>();

            workers.AddRange(ParseWorkers(command, result, WorkerType.Farmers, c => c.Farmers));
            workers.AddRange(ParseWorkers(command, result, WorkerType.WoodWorkers, c => c.WoodWorkers));
            workers.AddRange(ParseWorkers(command, result, WorkerType.StoneMasons, c => c.StoneMasons));
            workers.AddRange(ParseWorkers(command, result, WorkerType.OreMiners, c => c.OreMiners));
            workers.AddRange(ParseWorkers(command, result, WorkerType.SiegeEngineers, c => c.SiegeEngineers));
            workers.AddRange(ParseWorkers(command, result, WorkerType.Merchants, c => c.Merchants));

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