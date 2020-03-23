using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Repositories.Markets;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Markets {
    [InterfaceInjectable]
    [Audit]
    public sealed class WithdrawCaravanCommandHandler : ICommandHandler<WithdrawCaravanCommand> {
        private readonly IPlayerRepository _repository;
        private readonly ICaravanRepository _caravanRepository;

        public WithdrawCaravanCommandHandler(IPlayerRepository repository, ICaravanRepository caravanRepository) {
            _repository = repository;
            _caravanRepository = caravanRepository;
        }

        public CommandResult<WithdrawCaravanCommand> Execute(WithdrawCaravanCommand command) {
            var result = new CommandResult<WithdrawCaravanCommand>();
            var player = _repository.Get(command.Email);
            var caravanId = int.Parse(command.CaravanId);
            var caravan = player.Caravans.Single(c => c.Id == caravanId);

            caravan.Withdraw();
            _caravanRepository.Remove(caravan);
            _caravanRepository.Update();

            return result;
        }
    }
}