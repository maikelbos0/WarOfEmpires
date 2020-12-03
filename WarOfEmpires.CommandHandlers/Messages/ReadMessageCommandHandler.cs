﻿using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Messages;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Messages {
    [InterfaceInjectable]
    [Audit]
    public sealed class ReadMessageCommandHandler : ICommandHandler<ReadMessageCommand> {
        private readonly IPlayerRepository _repository;

        public ReadMessageCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<ReadMessageCommand> Execute(ReadMessageCommand command) {
            var result = new CommandResult<ReadMessageCommand>();
            var player = _repository.Get(command.Email);
            var message = player.ReceivedMessages.Single(m => m.Id == command.MessageId);

            message.IsRead = true;

            _repository.SaveChanges();

            return result;
        }
    }
}