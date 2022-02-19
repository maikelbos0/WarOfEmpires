using VDT.Core.DependencyInjection.Attributes;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [TransientServiceImplementation(typeof(ICommandHandler<PostChatMessageCommand>))]
    public sealed class PostChatMessageCommandHandler : ICommandHandler<PostChatMessageCommand> {
        private readonly IPlayerRepository _repository;

        public PostChatMessageCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<PostChatMessageCommand> Execute(PostChatMessageCommand command) {
            var result = new CommandResult<PostChatMessageCommand>();
            var player = _repository.Get(command.Email);

            player.Alliance.PostChatMessage(player, command.Message);
            _repository.SaveChanges();

            return result;
        }
    }
}