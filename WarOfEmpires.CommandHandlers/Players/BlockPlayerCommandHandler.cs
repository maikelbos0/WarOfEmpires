using System;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Players;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Players {
    [TransientServiceImplementation(typeof(ICommandHandler<BlockPlayerCommand>))]
    public sealed class BlockPlayerCommandHandler : ICommandHandler<BlockPlayerCommand> {
        private readonly IPlayerRepository _repository;

        public BlockPlayerCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<BlockPlayerCommand> Execute(BlockPlayerCommand command) {
            throw new NotImplementedException();
        }
    }
}
