using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Empires {
    public sealed class DepositCommandHandler : ICommandHandler<DepositCommand> {
        private readonly IPlayerRepository _repository;

        public DepositCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<DepositCommand> Execute(DepositCommand command) {
            var result = new CommandResult<DepositCommand>();
            var player = _repository.Get(command.Email);

            if (player.BankTurns <= 0) {
                result.AddError("You don't have any bank turns available");
            }

            if (result.Success) {
                player.Deposit();
                _repository.SaveChanges();
            }

            return result;
        }
    }
}