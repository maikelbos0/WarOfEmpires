﻿using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Messages;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Messages {
    [ScopedServiceImplementation(typeof(ICommandHandler<ReadMessageCommand>))]
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