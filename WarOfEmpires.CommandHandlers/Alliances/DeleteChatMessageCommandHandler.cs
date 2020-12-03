﻿using System.Linq;
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
            var result = new CommandResult<DeleteChatMessageCommand>();
            var alliance = _repository.Get(command.Email);
            var chatMessage = alliance.ChatMessages.Single(r => r.Id == command.ChatMessageId);

            alliance.DeleteChatMessage(chatMessage);
            _repository.SaveChanges();

            return result;
        }
    }
}