﻿using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
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
            int archers = 0;
            int mercenaryArchers = 0;
            int cavalry = 0;
            int mercenaryCavalry = 0;
            int footmen = 0;
            int mercenaryFootmen = 0;

            if (!string.IsNullOrEmpty(command.Archers) && !int.TryParse(command.Archers, out archers) || archers < 0) {
                result.AddError(c => c.Archers, "Archers must be a valid number");
            }

            if (!string.IsNullOrEmpty(command.MercenaryArchers) && !int.TryParse(command.MercenaryArchers, out mercenaryArchers) || mercenaryArchers < 0) {
                result.AddError(c => c.MercenaryArchers, "Archer mercenaries must be a valid number");
            }

            if (!string.IsNullOrEmpty(command.Cavalry) && !int.TryParse(command.Cavalry, out cavalry) || cavalry < 0) {
                result.AddError(c => c.Cavalry, "Cavalry must be a valid number");
            }

            if (!string.IsNullOrEmpty(command.MercenaryCavalry) && !int.TryParse(command.MercenaryCavalry, out mercenaryCavalry) || mercenaryCavalry < 0) {
                result.AddError(c => c.MercenaryCavalry, "Cavalry mercenaries must be a valid number");
            }

            if (!string.IsNullOrEmpty(command.Footmen) && !int.TryParse(command.Footmen, out footmen) || footmen < 0) {
                result.AddError(c => c.Footmen, "Footmen must be a valid number");
            }

            if (!string.IsNullOrEmpty(command.MercenaryFootmen) && !int.TryParse(command.MercenaryFootmen, out mercenaryFootmen) || mercenaryFootmen < 0) {
                result.AddError(c => c.MercenaryFootmen, "Footman mercenaries must be a valid number");
            }

            if (archers > player.Archers) {
                result.AddError(c => c.Archers, "You don't have that many archers to untrain");
            }

            if (mercenaryArchers > player.MercenaryArchers) {
                result.AddError(c => c.MercenaryArchers, "You don't have that many archer mercenaries to untrain");
            }

            if (cavalry > player.Cavalry) {
                result.AddError(c => c.Cavalry, "You don't have that many cavalry units to untrain");
            }

            if (mercenaryCavalry > player.MercenaryCavalry) {
                result.AddError(c => c.MercenaryCavalry, "You don't have that many cavalry mercenaries to untrain");
            }

            if (footmen > player.Footmen) {
                result.AddError(c => c.Footmen, "You don't have that many footmen to untrain");
            }

            if (mercenaryFootmen > player.MercenaryFootmen) {
                result.AddError(c => c.MercenaryFootmen, "You don't have that many footman mercenaries to untrain");
            }

            if (result.Success) {
                player.UntrainTroops(archers, mercenaryArchers, cavalry, mercenaryCavalry, footmen, mercenaryFootmen);
                _repository.Update();
            }

            return result;
        }
    }
}
