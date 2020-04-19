using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class UpdateRankCommandHandler : ICommandHandler<UpdateRankCommand> {
        private readonly IPlayerRepository _repository;
        private readonly RankService _rankService;

        public UpdateRankCommandHandler(IPlayerRepository repository, RankService rankService) {
            _repository = repository;
            _rankService = rankService;
        }

        public CommandResult<UpdateRankCommand> Execute(UpdateRankCommand command) {
            var result = new CommandResult<UpdateRankCommand>();

            _rankService.Update(_repository.GetAll());
            _repository.Update();

            return result;
        }
    }
}