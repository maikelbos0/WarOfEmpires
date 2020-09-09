using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Messages;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Messages {
    [InterfaceInjectable]
    [Audit]
    public sealed class SendMessageCommandHandler : ICommandHandler<SendMessageCommand> {
        private readonly IPlayerRepository _repository;

        public SendMessageCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<SendMessageCommand> Execute(SendMessageCommand command) {
            var result = new CommandResult<SendMessageCommand>();
            var sender = _repository.Get(command.SenderEmail);
            var recipient = _repository.Get(int.Parse(command.RecipientId));

            sender.SendMessage(recipient, command.Subject, command.Body);
            _repository.SaveChanges();

            return result;
        }
    }
}