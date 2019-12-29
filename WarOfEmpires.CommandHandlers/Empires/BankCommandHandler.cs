using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class BankCommandHandler : ICommandHandler<BankCommand> {
        public PlayerRepository _repository;

        public BankCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<BankCommand> Execute(BankCommand command) {
            var result = new CommandResult<BankCommand>();
            var player = _repository.Get(command.Email);

            if (player.BankTurns <= 0) {
                result.AddError("You don't have any bank turns available");
            }

            if (result.Success) {
                player.Bank();
                _repository.Update();
            }

            return result;
        }
    }
}