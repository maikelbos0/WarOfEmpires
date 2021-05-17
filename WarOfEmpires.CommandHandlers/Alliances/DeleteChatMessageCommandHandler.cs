using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [TransientServiceImplementation(typeof(ICommandHandler<DeleteChatMessageCommand>))]
    public sealed class DeleteChatMessageCommandHandler : ICommandHandler<DeleteChatMessageCommand> {
        private readonly IAllianceRepository _repository;

        public DeleteChatMessageCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<DeleteChatMessageCommand> Execute(DeleteChatMessageCommand command) {
            var result = new CommandResult<DeleteChatMessageCommand>();
            var alliance = _repository.Get(command.Email);
            var chatMessage = alliance.ChatMessages.Single(r => r.Id == command.ChatMessageId);

            alliance.DeleteChatMessage(chatMessage);
            _repository.SaveChanges();

            return result;
        }
    }
}