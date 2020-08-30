using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class HealTroopsCommandHandler : ICommandHandler<HealTroopsCommand> {
        private readonly IPlayerRepository _repository;

        public HealTroopsCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<HealTroopsCommand> Execute(HealTroopsCommand command) {
            var result = new CommandResult<HealTroopsCommand>();
            var player = _repository.Get(command.Email);
            int staminaToHeal = 0;

            if (!int.TryParse(command.StaminaToHeal, out staminaToHeal) || staminaToHeal < 0) {
                result.AddError(c => c.StaminaToHeal, "Stamina to heal must be a valid number");
                staminaToHeal = 0;
            }

            if ((player.Stamina + staminaToHeal) > 100) {
                result.AddError(c => c.StaminaToHeal, "You cannot heal above 100%");
            }

            if (!player.CanAfford(staminaToHeal * Player.HealCostPerTroopPerTurn * player.Troops.Sum(t => t.GetTotals()))) {
                result.AddError("You don't have enough food to heal these troops");
            }

            if (result.Success) {
                player.HealTroops(staminaToHeal);
                _repository.SaveChanges();
            }

            return result;
        }
    }
}