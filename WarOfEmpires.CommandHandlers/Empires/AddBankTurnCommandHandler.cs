using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class AddBankTurnCommandHandler : ICommandHandler<AddBankTurnCommand> {
        private readonly IPlayerRepository _repository;

        public AddBankTurnCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<AddBankTurnCommand> Execute(AddBankTurnCommand command) {
            var result = new CommandResult<AddBankTurnCommand>();

            foreach (var player in _repository.GetAll()) {
                player.AddBankTurn();
            }

            _repository.SaveChanges();

            return result;
        }
    }
}