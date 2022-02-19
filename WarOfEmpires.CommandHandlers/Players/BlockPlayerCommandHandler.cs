using VDT.Core.DependencyInjection.Attributes;
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
            var result = new CommandResult<BlockPlayerCommand>();
            var currentPlayer = _repository.Get(command.Email);
            var player = _repository.Get(command.PlayerId);

            currentPlayer.Block(player);

            _repository.SaveChanges();

            return result;
        }
    }
}
