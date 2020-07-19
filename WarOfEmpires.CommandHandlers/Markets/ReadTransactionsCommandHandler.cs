using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Markets {
    [InterfaceInjectable]
    [Audit]
    public sealed class ReadTransactionsCommandHandler : ICommandHandler<ReadTransactionsCommand> {
        private readonly IPlayerRepository _repository;

        public ReadTransactionsCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<ReadTransactionsCommand> Execute(ReadTransactionsCommand command) {
            var result = new CommandResult<ReadTransactionsCommand>();
            var player = _repository.Get(command.Email);

            player.HasNewMarketSales = false;

            if (player.SellTransactions.Any(t => !t.IsRead)) {
                foreach (var transaction in player.SellTransactions.Where(t => !t.IsRead)) {
                    transaction.IsRead = true;
                }

                _repository.Update();
            }

            return result;
        }
    }
}