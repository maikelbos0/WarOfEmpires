using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Markets {
    [InterfaceInjectable]
    [Audit]
    public sealed class WithdrawCaravanCommandHandler : ICommandHandler<WithdrawCaravanCommand> {
        private readonly IPlayerRepository _repository;

        public WithdrawCaravanCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<WithdrawCaravanCommand> Execute(WithdrawCaravanCommand command) {
            var result = new CommandResult<WithdrawCaravanCommand>();
            var player = _repository.Get(command.Email);
            var caravanId = int.Parse(command.CaravanId);
            var caravan = player.Caravans.Single(c => c.Id == caravanId);

            caravan.Withdraw();
            _repository.Update();

            return result;
        }
    }
}