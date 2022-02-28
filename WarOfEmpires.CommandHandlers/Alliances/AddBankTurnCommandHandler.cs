using WarOfEmpires.Utilities.Auditing;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;

namespace WarOfEmpires.CommandHandlers.Alliances {
    public sealed class AddBankTurnCommandHandler : ICommandHandler<AddBankTurnCommand> {
        private readonly IAllianceRepository _repository;

        public AddBankTurnCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<AddBankTurnCommand> Execute(AddBankTurnCommand command) {
            var result = new CommandResult<AddBankTurnCommand>();

            foreach (var alliance in _repository.GetAll()) {
                alliance.AddBankTurn();
            }

            _repository.SaveChanges();

            return result;
        }
    }
}