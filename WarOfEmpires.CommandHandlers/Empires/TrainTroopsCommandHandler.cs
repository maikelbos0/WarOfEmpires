using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Linq;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class TrainTroopsCommandHandler : ICommandHandler<TrainTroopsCommand> {
        public PlayerRepository _repository;

        public TrainTroopsCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<TrainTroopsCommand> Execute(TrainTroopsCommand command) {
            var result = new CommandResult<TrainTroopsCommand>();
            var player = _repository.Get(command.Email);
            var troops = new List<TroopInfo>();
            int soldiers = 0;
            int mercenaries = 0;

            // Archers
            if (!string.IsNullOrEmpty(command.Archers) && !int.TryParse(command.Archers, out soldiers) || soldiers < 0) {
                result.AddError(c => c.Archers, "Archers must be a valid number");
                soldiers = 0;
            }

            if (!string.IsNullOrEmpty(command.MercenaryArchers) && !int.TryParse(command.MercenaryArchers, out mercenaries) || mercenaries < 0) {
                result.AddError(c => c.MercenaryArchers, "Archer mercenaries must be a valid number");
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

            if (!string.IsNullOrEmpty(command.MercenaryCavalry) && !int.TryParse(command.MercenaryCavalry, out mercenaries) || mercenaries < 0) {
                result.AddError(c => c.MercenaryCavalry, "Cavalry mercenaries must be a valid number");
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

            if (!string.IsNullOrEmpty(command.MercenaryFootmen) && !int.TryParse(command.MercenaryFootmen, out mercenaries) || mercenaries < 0) {
                result.AddError(c => c.MercenaryFootmen, "Footman mercenaries must be a valid number");
                mercenaries = 0;
            }

            if (soldiers > 0 || mercenaries > 0) {
                troops.Add(new TroopInfo(TroopType.Footmen, soldiers, mercenaries));
            }

            if (troops.Sum(t => t.Soldiers) > player.Peasants) {
                result.AddError("You don't have that many peasants available to train");
            }

            if (troops.Sum(t => t.Soldiers + t.Mercenaries) > player.GetAvailableBarracksCapacity()) {
                result.AddError("You don't have enough barracks available to train that many troops");
            }

            if (!player.CanAfford(troops.Sum(t => TroopDefinitionFactory.Get(t.Type).Cost * t.Soldiers)
                + troops.Sum(t => t.Mercenaries) * Player.MercenaryTrainingCost)) {
                result.AddError("You don't have enough resources to train these troops");
            }

            if (result.Success) {
                foreach (var t in troops) {
                    player.TrainTroops(t.Type, t.Soldiers, t.Mercenaries);
                }

                _repository.Update();
            }

            return result;
        }
    }
}
