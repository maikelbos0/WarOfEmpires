using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Messages;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Messages {
    [InterfaceInjectable]
    [Audit]
    public sealed class SendMessageCommandHandler : ICommandHandler<SendMessageCommand> {
        private readonly PlayerRepository _repository;

        public SendMessageCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<SendMessageCommand> Execute(SendMessageCommand command) {
            throw new System.NotImplementedException();
        }
    }
}