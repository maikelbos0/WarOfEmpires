using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class PostChatMessageCommandHandler : ICommandHandler<PostChatMessageCommand> {
        private readonly IPlayerRepository _repository;

        public PostChatMessageCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<PostChatMessageCommand> Execute(PostChatMessageCommand command) {
            throw new System.NotImplementedException();
        }
    }
}