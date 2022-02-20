using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Markets {
    public sealed class WithdrawCaravanCommandHandler : ICommandHandler<WithdrawCaravanCommand> {
        private readonly IPlayerRepository _repository;

        public WithdrawCaravanCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<WithdrawCaravanCommand> Execute(WithdrawCaravanCommand command) {
            var result = new CommandResult<WithdrawCaravanCommand>();
            var player = _repository.Get(command.Email);
            var caravan = player.Caravans.Single(c => c.Id == command.CaravanId);

            caravan.Withdraw();
            _repository.SaveChanges();

            return result;
        }
    }
}