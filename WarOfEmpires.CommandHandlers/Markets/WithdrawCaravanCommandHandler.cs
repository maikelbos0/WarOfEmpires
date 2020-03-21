using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Repositories.Markets;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Markets {
    public sealed class WithdrawCaravanCommandHandler : ICommandHandler<WithdrawCaravanCommand> {
        private readonly IPlayerRepository _repository;
        private readonly ICaravanRepository _caravanRepository;

        public WithdrawCaravanCommandHandler(IPlayerRepository repository, ICaravanRepository caravanRepository) {
            _repository = repository;
            _caravanRepository = caravanRepository;
        }

        public CommandResult<WithdrawCaravanCommand> Execute(WithdrawCaravanCommand command) {
            throw new System.NotImplementedException();
        }
    }
}