using System;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class UntrainTroopsCommandHandler : ICommandHandler<UntrainTroopsCommand> {
        private readonly IPlayerRepository _repository;
        private readonly EnumFormatter _formatter;

        public UntrainTroopsCommandHandler(IPlayerRepository repository, EnumFormatter formatter) {
            _repository = repository;
            _formatter = formatter;
        }

        public CommandResult<UntrainTroopsCommand> Execute(UntrainTroopsCommand command) {
            var result = new CommandResult<UntrainTroopsCommand>();
            var player = _repository.Get(command.Email);
            var troops = new List<TroopInfo>();

            for (var index = 0; index < command.Troops.Count; index++) {
                var i = index; // Don't use iterator in lambdas
                var type = (TroopType)Enum.Parse(typeof(TroopType), command.Troops[i].Type);
                int soldiers = 0;
                int mercenaries = 0;

                if (!string.IsNullOrEmpty(command.Troops[i].Soldiers) && !int.TryParse(command.Troops[i].Soldiers, out soldiers) || soldiers < 0) {
                    result.AddError(c => c.Troops[i].Soldiers, "Invalid number");
                }
                else if (soldiers > player.GetTroops(type).Soldiers) {
                    result.AddError(c => c.Troops[i].Soldiers, $"You don't have that many {_formatter.ToString(type, false)} to untrain");
                }

                if (!string.IsNullOrEmpty(command.Troops[i].Mercenaries) && !int.TryParse(command.Troops[i].Mercenaries, out mercenaries) || mercenaries < 0) {
                    result.AddError(c => c.Troops[i].Mercenaries, "Invalid number");
                }
                else if (mercenaries > player.GetTroops(type).Mercenaries) {
                    result.AddError(c => c.Troops[i].Mercenaries, $"You don't have that many mercenary {_formatter.ToString(type, false)} to untrain");
                }

                if (result.Success && (soldiers > 0 || mercenaries > 0)) {
                    troops.Add(new TroopInfo(type, soldiers, mercenaries));
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
