using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

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
            int archers = 0;
            int mercenaryArchers = 0;
            int cavalry = 0;
            int mercenaryCavalry = 0;
            int footmen = 0;
            int mercenaryFootmen = 0;

            if (!string.IsNullOrEmpty(command.Archers) && !int.TryParse(command.Archers, out archers) || archers < 0) {
                result.AddError(c => c.Archers, "Archers must be a valid number");
                archers = 0;
            }

            if (!string.IsNullOrEmpty(command.MercenaryArchers) && !int.TryParse(command.MercenaryArchers, out mercenaryArchers) || mercenaryArchers < 0) {
                result.AddError(c => c.MercenaryArchers, "Archer mercenaries must be a valid number");
                mercenaryArchers = 0;
            }

            if (!string.IsNullOrEmpty(command.Cavalry) && !int.TryParse(command.Cavalry, out cavalry) || cavalry < 0) {
                result.AddError(c => c.Cavalry, "Cavalry must be a valid number");
                cavalry = 0;
            }

            if (!string.IsNullOrEmpty(command.MercenaryCavalry) && !int.TryParse(command.MercenaryCavalry, out mercenaryCavalry) || mercenaryCavalry < 0) {
                result.AddError(c => c.MercenaryCavalry, "Cavalry mercenaries must be a valid number");
                mercenaryCavalry = 0;
            }

            if (!string.IsNullOrEmpty(command.Footmen) && !int.TryParse(command.Footmen, out footmen) || footmen < 0) {
                result.AddError(c => c.Footmen, "Footmen must be a valid number");
                footmen = 0;
            }

            if (!string.IsNullOrEmpty(command.MercenaryFootmen) && !int.TryParse(command.MercenaryFootmen, out mercenaryFootmen) || mercenaryFootmen < 0) {
                result.AddError(c => c.MercenaryFootmen, "Footman mercenaries must be a valid number");
                mercenaryFootmen = 0;
            }

            if (archers + cavalry + footmen > player.Peasants) {
                result.AddError("You don't have that many peasants available to train");
            }

            if (archers + cavalry + footmen + mercenaryArchers + mercenaryCavalry + mercenaryFootmen > player.GetAvailableBarracksCapacity()) {
                result.AddError("You don't have enough barracks available to train that many troops");
            }

            if (!player.CanAfford((archers * Player.ArcherTrainingCost) + (cavalry * Player.CavalryTrainingCost) + (footmen * Player.FootmanTrainingCost) + ((mercenaryArchers + mercenaryCavalry + mercenaryFootmen) * Player.MercenaryTrainingCost))) {
                result.AddError("You don't have enough resources to train these troops");
            }

            if (result.Success) {
                player.TrainTroops(TroopType.Archers, archers, mercenaryArchers);
                player.TrainTroops(TroopType.Cavalry, cavalry, mercenaryCavalry);
                player.TrainTroops(TroopType.Footmen, footmen, mercenaryFootmen);
                _repository.Update();
            }

            return result;
        }
    }
}
