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

        public CommandResult<UntrainWorkersCommand> Execute(UntrainWorkersCommand command) {
            var result = new CommandResult<UntrainWorkersCommand>();
            var player = _repository.Get(command.Email);
            var workers = new List<WorkerInfo>();

            for (var index = 0; index < command.Workers.Count; index++) {
                var i = index; // Don't use iterator in lambdas
                var type = (WorkerType)Enum.Parse(typeof(WorkerType), command.Workers[i].Type);
                int count = 0;

                if (!string.IsNullOrEmpty(command.Workers[i].Count) && !int.TryParse(command.Workers[i].Count, out count) || count < 0) {
                    result.AddError(c => c.Workers[i].Count, "Invalid number");
                }
                else if (count > player.GetWorkerCount(type)) {
                    result.AddError(c => c.Workers[i].Count, $"You don't have that many {_formatter.ToString(type, false)} to untrain");
                }

                if (type == WorkerType.SiegeEngineers && count > 0 && (player.GetWorkerCount(WorkerType.SiegeEngineers) - count) * player.GetBuildingBonus(BuildingType.SiegeFactory) < player.SiegeWeapons.Sum(s => s.Count * SiegeWeaponDefinitionFactory.Get(s.Type).Maintenance)) {
                    result.AddError(c => c.Workers[i].Count, "Your siege engineers are maintaining too many siege weapons for that many to be untrained");
                }

                if (type == WorkerType.Merchants && count > 0 && player.GetWorkerCount(WorkerType.Merchants) - count < player.Caravans.Count) {
                    result.AddError(c => c.Workers[i].Count, "You can not untrain merchants that have a caravan on the market");
                }

                if (result.Success && count > 0) {
                    workers.Add(new WorkerInfo(type, count));
                }
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