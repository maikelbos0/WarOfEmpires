using System.Linq;
using VDT.Core.DependencyInjection.Attributes;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Empires {
    [TransientServiceImplementation(typeof(ICommandHandler<HealTroopsCommand>))]
    public sealed class HealTroopsCommandHandler : ICommandHandler<HealTroopsCommand> {
        private readonly IPlayerRepository _repository;

        public HealTroopsCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<HealTroopsCommand> Execute(HealTroopsCommand command) {
            var result = new CommandResult<HealTroopsCommand>();
            var player = _repository.Get(command.Email);
            
            if ((player.Stamina + command.StaminaToHeal) > 100) {
                result.AddError(c => c.StaminaToHeal, "You cannot heal above 100%");
            }

            if (!player.CanAfford(command.StaminaToHeal * Player.HealCostPerTroopPerTurn * player.Troops.Sum(t => t.GetTotals()))) {
                result.AddError("You don't have enough food to heal these troops");
            }

            if (result.Success) {
                player.HealTroops(command.StaminaToHeal);
                _repository.SaveChanges();
            }

            return result;
        }
    }
}