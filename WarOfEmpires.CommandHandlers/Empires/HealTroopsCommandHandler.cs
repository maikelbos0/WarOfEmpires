using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class HealTroopsCommandHandler : ICommandHandler<HealTroopsCommand> {
        public PlayerRepository _repository;

        public HealTroopsCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<HealTroopsCommand> Execute(HealTroopsCommand command) {
            var result = new CommandResult<HealTroopsCommand>();
            var player = _repository.Get(command.Email);
            int staminaToHeal = 0;

            if (!string.IsNullOrEmpty(command.StaminaToHeal) && !int.TryParse(command.StaminaToHeal, out staminaToHeal) || staminaToHeal < 0) {
                result.AddError(c => c.StaminaToHeal, "Stamina to heal must be a valid number");
            }

            if ((player.Stamina + staminaToHeal) > 100) {
                result.AddError(c => c.StaminaToHeal, "You cannot heal above 100%");
            }

            if (result.Success) {
                player.HealTroops(staminaToHeal);
                _repository.Update();
            }

            // TODO: Use the the code below to figure out something for whether you can afford to heal what you wanted or not

            //if (!player.CanAfford(healTurnsStamina * Player.HealCostPerTroopPerTurn * (Player.Archers.GetTotals() + Player.Cavalry.GetTotals() + Player.Footmen.GetTotals()) {
            //    result.AddError("You don't have enough food to heal these troops");
            //}

            //if (!player.CanAfford((archers * Player.ArcherTrainingCost) + (cavalry * Player.CavalryTrainingCost) + (footmen * Player.FootmanTrainingCost) + ((mercenaryArchers + mercenaryCavalry + mercenaryFootmen) * Player.MercenaryTrainingCost))) {
            //    result.AddError("You don't have enough resources to train these troops");
            //}


            return result;
        }
    }
}