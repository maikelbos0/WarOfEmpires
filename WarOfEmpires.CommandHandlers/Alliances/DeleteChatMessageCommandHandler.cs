using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class DeleteChatMessageCommandHandler : ICommandHandler<DeleteChatMessageCommand> {
        private readonly IAllianceRepository _repository;

        public DeleteChatMessageCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        public CommandResult<DeleteChatMessageCommand> Execute(DeleteChatMessageCommand command) {
            throw new System.NotImplementedException();
        }
    }
}