using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Alliances {

    [TransientServiceImplementation(typeof(ICommandHandler<WithdrawCommand>))]
    public sealed class WithdrawCommandHandler : ICommandHandler<WithdrawCommand> {
        private readonly IPlayerRepository _repository;

        public WithdrawCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<WithdrawCommand> Execute(WithdrawCommand command) {
            var result = new CommandResult<WithdrawCommand>();
            var player = _repository.Get(command.Email);
            var alliance = player.Alliance;
            var resources = new Resources(command.Gold ?? 0, command.Food ?? 0, command.Wood ?? 0, command.Stone ?? 0, command.Ore ?? 0);

            if (!alliance.BankedResources.CanAfford(resources)) {
                result.AddError("Your alliance doesn't have enough resources available to withdraw that much");
            }

            if (alliance.BankTurns <= 0) {
                result.AddError("Your alliance doesn't have any bank turns available");
            }

            if (result.Success) {
                alliance.Withdraw(player, resources);
                _repository.SaveChanges();
            }

            return result;
        }
    }
}
