using WarOfEmpires.Utilities.Auditing;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Markets {
    public sealed class ReadTransactionsCommandHandler : ICommandHandler<ReadTransactionsCommand> {
        private readonly IPlayerRepository _repository;

        public ReadTransactionsCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<ReadTransactionsCommand> Execute(ReadTransactionsCommand command) {
            var result = new CommandResult<ReadTransactionsCommand>();
            var player = _repository.Get(command.Email);

            player.HasNewMarketSales = false;
            _repository.SaveChanges();

            return result;
        }
    }
}