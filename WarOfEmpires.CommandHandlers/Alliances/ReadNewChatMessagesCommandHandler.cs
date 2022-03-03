using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Alliances {
    public sealed class ReadNewChatMessagesCommandHandler : ICommandHandler<ReadNewChatMessagesCommand> {
        private readonly IPlayerRepository _repository;

        public ReadNewChatMessagesCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<ReadNewChatMessagesCommand> Execute(ReadNewChatMessagesCommand command) {
            var result = new CommandResult<ReadNewChatMessagesCommand>();
            var player = _repository.Get(command.Email);

            player.HasNewChatMessages = false;
            _repository.SaveChanges();

            return result;
        }
    }
}