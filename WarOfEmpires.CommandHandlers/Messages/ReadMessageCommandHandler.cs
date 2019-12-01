using System;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Messages;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Messages {
    [InterfaceInjectable]
    [Audit]
    public sealed class ReadMessageCommandHandler : ICommandHandler<ReadMessageCommand> {
        private readonly PlayerRepository _repository;

        public ReadMessageCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<ReadMessageCommand> Execute(ReadMessageCommand command) {
            throw new NotImplementedException();
        }
    }
}