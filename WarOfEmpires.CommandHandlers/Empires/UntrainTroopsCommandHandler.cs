using System;
using System.Collections.Generic;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.CommandHandlers.Empires {
    [ScopedServiceImplementation(typeof(ICommandHandler<UntrainTroopsCommand>))]
    public sealed class UntrainTroopsCommandHandler : ICommandHandler<UntrainTroopsCommand> {
        private readonly IPlayerRepository _repository;
        private readonly IEnumFormatter _formatter;

        public UntrainTroopsCommandHandler(IPlayerRepository repository, IEnumFormatter formatter) {
            _repository = repository;
            _formatter = formatter;
        }

        [Audit]
        public CommandResult<UntrainTroopsCommand> Execute(UntrainTroopsCommand command) {
            var result = new CommandResult<UntrainTroopsCommand>();
            var player = _repository.Get(command.Email);
            var troops = new List<TroopInfo>();

            for (var index = 0; index < command.Troops.Count; index++) {
                var i = index; // Don't use iterator in lambdas
                var type = (TroopType)Enum.Parse(typeof(TroopType), command.Troops[i].Type);
                
                if (command.Troops[i].Soldiers.HasValue && command.Troops[i].Soldiers.Value > player.GetTroops(type).Soldiers) {
                    result.AddError(c => c.Troops[i].Soldiers, $"You don't have that many {_formatter.ToString(type, false)} to untrain");
                }

                if (command.Troops[i].Mercenaries.HasValue && command.Troops[i].Mercenaries.Value > player.GetTroops(type).Mercenaries) {
                    result.AddError(c => c.Troops[i].Mercenaries, $"You don't have that many mercenary {_formatter.ToString(type, false)} to untrain");
                }

                if (result.Success && (command.Troops[i].Soldiers.HasValue || command.Troops[i].Mercenaries.HasValue)) {
                    troops.Add(new TroopInfo(type, command.Troops[i].Soldiers ?? 0, command.Troops[i].Mercenaries ?? 0));
                }
            }

            if (result.Success) {
                foreach (var t in troops) {
                    player.UntrainTroops(t.Type, t.Soldiers, t.Mercenaries);
                }

                _repository.SaveChanges();
            }

            return result;
        }
    }
}
