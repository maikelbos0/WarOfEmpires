using WarOfEmpires.Commands.Players;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Players {
    public sealed class UnblockPlayerCommandHandler : ICommandHandler<UnblockPlayerCommand> {
        private readonly IPlayerRepository _repository;

        public UnblockPlayerCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<UnblockPlayerCommand> Execute(UnblockPlayerCommand command) {
            var result = new CommandResult<UnblockPlayerCommand>();
            var currentPlayer = _repository.Get(command.Email);
            var player = _repository.Get(command.PlayerId);

            currentPlayer.Unblock(player);

            _repository.SaveChanges();

            return result;
        }
    }
}
