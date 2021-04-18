using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Empires {
    [ScopedServiceImplementation(typeof(ICommandHandler<AddBankTurnCommand>))]
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