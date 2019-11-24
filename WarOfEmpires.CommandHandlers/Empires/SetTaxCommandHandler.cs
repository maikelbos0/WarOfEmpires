using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class SetTaxCommandHandler : ICommandHandler<SetTaxCommand> {
        public PlayerRepository _repository;

        public SetTaxCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<SetTaxCommand> Execute(SetTaxCommand command) {
            var result = new CommandResult<SetTaxCommand>();
            var player = _repository.Get(command.Email);
            int tax;

            if (!int.TryParse(command.Tax, out tax) || tax < 0 || tax > 100) {
                result.AddError(c => c.Tax, "Tax must be a valid number");
            }

            if (result.Success) {
                player.Tax = tax;
                _repository.Update();
            }

            return result;
        }
    }
}