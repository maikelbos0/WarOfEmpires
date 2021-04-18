using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Empires {
    [ScopedServiceImplementation(typeof(ICommandHandler<BankCommand>))]
    [Audit]
    public sealed class BankCommandHandler : ICommandHandler<BankCommand> {
        private readonly IPlayerRepository _repository;

        public BankCommandHandler(IPlayerRepository repository) {
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
                _repository.SaveChanges();
            }

            return result;
        }
    }
}