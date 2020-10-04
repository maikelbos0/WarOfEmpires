using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class RemoveChatMessageCommandHandler : ICommandHandler<RemoveChatMessageCommand> {
        private readonly IPlayerRepository _repository;

        public RemoveChatMessageCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<RemoveChatMessageCommand> Execute(RemoveChatMessageCommand command) {
            throw new System.NotImplementedException();
        }
    }
}