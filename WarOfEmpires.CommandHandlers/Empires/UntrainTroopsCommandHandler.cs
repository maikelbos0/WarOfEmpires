using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class UntrainTroopsCommandHandler : ICommandHandler<UntrainTroopsCommand> {
        public PlayerRepository _repository;

        public UntrainTroopsCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<UntrainTroopsCommand> Execute(UntrainTroopsCommand command) {
            var result = new CommandResult<UntrainTroopsCommand>();
            var player = _repository.Get(command.Email);
            var troops = new List<TroopInfo>();
            int soldiers = 0;
            int mercenaries = 0;

            // Archers
            if (!string.IsNullOrEmpty(command.Archers) && !int.TryParse(command.Archers, out soldiers) || soldiers < 0) {
                result.AddError(c => c.Archers, "Archers must be a valid number");
                soldiers = 0;
            }
            else if (soldiers > player.GetTroops(TroopType.Archers).Soldiers) {
                result.AddError(c => c.Archers, "You don't have that many archers to untrain");
                soldiers = 0;
            }

            if (!string.IsNullOrEmpty(command.MercenaryArchers) && !int.TryParse(command.MercenaryArchers, out mercenaries) || mercenaries < 0) {
                result.AddError(c => c.MercenaryArchers, "Archer mercenaries must be a valid number");
                mercenaries = 0;
            }
            else if (mercenaries > player.GetTroops(TroopType.Archers).Mercenaries) {
                result.AddError(c => c.MercenaryArchers, "You don't have that many archer mercenaries to untrain");
                mercenaries = 0;
            }

            if (soldiers > 0 || mercenaries > 0) {
                troops.Add(new TroopInfo(TroopType.Archers, soldiers, mercenaries));
            }

            // Cavalry
            if (!string.IsNullOrEmpty(command.Cavalry) && !int.TryParse(command.Cavalry, out soldiers) || soldiers < 0) {
                result.AddError(c => c.Cavalry, "Cavalry must be a valid number");
                soldiers = 0;
            }
            else if (soldiers > player.GetTroops(TroopType.Cavalry).Soldiers) {
                result.AddError(c => c.Cavalry, "You don't have that many cavalry units to untrain");
                soldiers = 0;
            }

            if (!string.IsNullOrEmpty(command.MercenaryCavalry) && !int.TryParse(command.MercenaryCavalry, out mercenaries) || mercenaries < 0) {
                result.AddError(c => c.MercenaryCavalry, "Cavalry mercenaries must be a valid number");
                mercenaries = 0;
            }
            else if (mercenaries > player.GetTroops(TroopType.Cavalry).Mercenaries) {
                result.AddError(c => c.MercenaryCavalry, "You don't have that many cavalry mercenaries to untrain");
                mercenaries = 0;
            }

            if (soldiers > 0 || mercenaries > 0) {
                troops.Add(new TroopInfo(TroopType.Cavalry, soldiers, mercenaries));
            }

            // Footmen
            if (!string.IsNullOrEmpty(command.Footmen) && !int.TryParse(command.Footmen, out soldiers) || soldiers < 0) {
                result.AddError(c => c.Footmen, "Footmen must be a valid number");
                soldiers = 0;
            }
            else if (soldiers > player.GetTroops(TroopType.Footmen).Soldiers) {
                result.AddError(c => c.Footmen, "You don't have that many footmen to untrain");
                soldiers = 0;
            }

            if (!string.IsNullOrEmpty(command.MercenaryFootmen) && !int.TryParse(command.MercenaryFootmen, out mercenaries) || mercenaries < 0) {
                result.AddError(c => c.MercenaryFootmen, "Footman mercenaries must be a valid number");
                mercenaries = 0;
            }
            else if (mercenaries > player.GetTroops(TroopType.Footmen).Mercenaries) {
                result.AddError(c => c.MercenaryFootmen, "You don't have that many footman mercenaries to untrain");
                mercenaries = 0;
            }

            if (soldiers > 0 || mercenaries > 0) {
                troops.Add(new TroopInfo(TroopType.Footmen, soldiers, mercenaries));
            }

            if (result.Success) {
                foreach (var t in troops) {
                    player.UntrainTroops(t.Type, t.Soldiers, t.Mercenaries);
                }

                _repository.Update();
            }

            return result;
        }
    }
}
