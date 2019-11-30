using System;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Messages;
using WarOfEmpires.Domain.Players;
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
            var result = new CommandResult<SendMessageCommand>();
            var sender = _repository.Get(command.SenderEmail);
            int recipientId;

            if (!int.TryParse(command.RecipientId, out recipientId)) {
                throw new InvalidOperationException($"Value '{command.RecipientId}' is not a valid player ID");
            }

            var recipient = _repository.Get(recipientId);
            var message = new Message(sender, recipient, command.Subject, command.Body);

            sender.SentMessages.Add(message);
            recipient.ReceivedMessages.Add(message);

            _repository.Update();

            return result;
        }
    }
}