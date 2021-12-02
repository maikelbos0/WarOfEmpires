using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Alliances {

    [TransientServiceImplementation(typeof(ICommandHandler<BankCommand>))]
    public sealed class BankCommandHandler : ICommandHandler<BankCommand> {
        private readonly IPlayerRepository _repository;

        public BankCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<BankCommand> Execute(BankCommand command) {
            var result = new CommandResult<BankCommand>();
            var player = _repository.Get(command.Email);
            var alliance = player.Alliance;
            var resources = new Resources(command.Gold ?? 0, command.Food ?? 0, command.Wood ?? 0, command.Stone ?? 0, command.Ore ?? 0);

            if (!player.CanAfford(resources)) {
                result.AddError("You don't have enough resources available to bank that much");
            }

            if (alliance.BankTurns <= 0) {
                result.AddError("You don't have any bank turns available");
            }

            if (result.Success) {
                alliance.Bank(player, resources);
                _repository.SaveChanges();
            }

            return result;
        }
    }
}
