using System;
using System.Collections.Generic;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Siege;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.CommandHandlers.Empires {
    [TransientServiceImplementation(typeof(ICommandHandler<UntrainWorkersCommand>))]
    public sealed class UntrainWorkersCommandHandler : ICommandHandler<UntrainWorkersCommand> {
        private readonly IPlayerRepository _repository;
        private readonly IEnumFormatter _formatter;

        public UntrainWorkersCommandHandler(IPlayerRepository repository, IEnumFormatter formatter) {
            _repository = repository;
            _formatter = formatter;
        }

        [Audit]
        public CommandResult<UntrainWorkersCommand> Execute(UntrainWorkersCommand command) {
            var result = new CommandResult<UntrainWorkersCommand>();
            var player = _repository.Get(command.Email);
            var workers = new List<WorkerInfo>();

            for (var index = 0; index < command.Workers.Count; index++) {
                var i = index; // Don't use iterator in lambdas
                var type = (WorkerType)Enum.Parse(typeof(WorkerType), command.Workers[i].Type);
                
                if (command.Workers[i].Count.HasValue && command.Workers[i].Count.Value > player.GetWorkerCount(type)) {
                    result.AddError(c => c.Workers[i].Count, $"You don't have that many {_formatter.ToString(type, false)} to untrain");
                }

                if (type == WorkerType.SiegeEngineers && command.Workers[i].Count.HasValue && (player.GetWorkerCount(WorkerType.SiegeEngineers) - command.Workers[i].Count.Value) * player.GetBuildingBonus(BuildingType.SiegeFactory) < player.SiegeWeapons.Sum(s => s.Count * SiegeWeaponDefinitionFactory.Get(s.Type).Maintenance)) {
                    result.AddError(c => c.Workers[i].Count, "Your siege engineers are maintaining too many siege weapons for that many to be untrained");
                }

                if (type == WorkerType.Merchants && command.Workers[i].Count.HasValue && player.GetWorkerCount(WorkerType.Merchants) - command.Workers[i].Count.Value < player.Caravans.Count) {
                    result.AddError(c => c.Workers[i].Count, "You can't untrain merchants that have a caravan on the market");
                }

                if (result.Success && command.Workers[i].Count.HasValue) {
                    workers.Add(new WorkerInfo(type, command.Workers[i].Count.Value));
                }
            }

            if (result.Success) {
                foreach (var workerInfo in workers) {
                    player.UntrainWorkers(workerInfo.Type, workerInfo.Count);
                }

                _repository.SaveChanges();
            }

            return result;
        }
    }
}