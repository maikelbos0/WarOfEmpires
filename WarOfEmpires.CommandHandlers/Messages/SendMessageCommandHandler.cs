﻿using System.Linq;
using WarOfEmpires.Commands.Messages;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Messages {
    public sealed class SendMessageCommandHandler : ICommandHandler<SendMessageCommand> {
        private readonly IPlayerRepository _repository;

        public SendMessageCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<SendMessageCommand> Execute(SendMessageCommand command) {
            var result = new CommandResult<SendMessageCommand>();
            var sender = _repository.Get(command.SenderEmail);
            var recipient = _repository.Get(command.RecipientId);

            if (recipient.PlayerBlocks.Any(b => b.BlockedPlayer == sender)) {
                result.AddError("You have been blocked from sending this player messages");
            }

            if (result.Success) {
                sender.SendMessage(recipient, command.Subject, command.Body);
                _repository.SaveChanges();
            }

            return result;
        }
    }
}