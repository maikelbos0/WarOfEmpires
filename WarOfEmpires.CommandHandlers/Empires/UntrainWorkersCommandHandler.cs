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
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class UntrainWorkersCommandHandler : ICommandHandler<UntrainWorkersCommand> {
        private readonly IPlayerRepository _repository;
        private readonly EnumFormatter _formatter;

        public UntrainWorkersCommandHandler(IPlayerRepository repository, EnumFormatter formatter) {
            _repository = repository;
            _formatter = formatter;
        }

        private IEnumerable<WorkerInfo> ParseWorkers(UntrainWorkersCommand command,
                                                     CommandResult<UntrainWorkersCommand> result,
                                                     WorkerType type,
                                                     Expression<Func<UntrainWorkersCommand, object>> workerFunc,
                                                     int maximumWorkers) {

            var commandWorkers = (string)workerFunc.Compile().Invoke(command);
            int workers = 0;

            if (!string.IsNullOrEmpty(commandWorkers) && !int.TryParse(commandWorkers, out workers) || workers < 0) {
                result.AddError(workerFunc, $"{_formatter.ToString(type)} must be a valid number");
            }
            else if (workers > maximumWorkers) {
                result.AddError(workerFunc, $"You don't have that many {_formatter.ToString(type, false)} to untrain");
            }

            if (result.Success && workers > 0) {
                yield return new WorkerInfo(type, workers);
            }
        }

        public CommandResult<UntrainWorkersCommand> Execute(UntrainWorkersCommand command) {
            var result = new CommandResult<UntrainWorkersCommand>();
            var player = _repository.Get(command.Email);
            var workers = new List<WorkerInfo>();

            workers.AddRange(ParseWorkers(command, result, WorkerType.Farmers, c => c.Farmers, player.GetWorkerCount(WorkerType.Farmers)));
            workers.AddRange(ParseWorkers(command, result, WorkerType.WoodWorkers, c => c.WoodWorkers, player.GetWorkerCount(WorkerType.WoodWorkers)));
            workers.AddRange(ParseWorkers(command, result, WorkerType.StoneMasons, c => c.StoneMasons, player.GetWorkerCount(WorkerType.StoneMasons)));
            workers.AddRange(ParseWorkers(command, result, WorkerType.OreMiners, c => c.OreMiners, player.GetWorkerCount(WorkerType.OreMiners)));
            workers.AddRange(ParseWorkers(command, result, WorkerType.SiegeEngineers, c => c.SiegeEngineers, player.GetWorkerCount(WorkerType.SiegeEngineers)));
            workers.AddRange(ParseWorkers(command, result, WorkerType.Merchants, c => c.Merchants, player.GetWorkerCount(WorkerType.Merchants)));

            var siegeEngineers = workers.SingleOrDefault(w => w.Type == WorkerType.SiegeEngineers)?.Count ?? 0;

            if (siegeEngineers > 0 && (player.GetWorkerCount(WorkerType.SiegeEngineers) - siegeEngineers) * player.GetBuildingBonus(BuildingType.SiegeFactory) < player.SiegeWeapons.Sum(s => s.Count * SiegeWeaponDefinitionFactory.Get(s.Type).Maintenance)) {
                result.AddError(c => c.SiegeEngineers, "Your siege engineers are maintaining too many siege weapons for that many to be untrained");
            }

            var merchants = workers.SingleOrDefault(w => w.Type == WorkerType.Merchants)?.Count ?? 0;

            if (merchants > 0 && player.GetWorkerCount(WorkerType.Merchants) - merchants < player.Caravans.Count) {
                result.AddError(c => c.Merchants, "You can not untrain merchants that have a caravan on the market");
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