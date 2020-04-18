using System.Linq;
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
            var rank = 1;

            foreach (var player in _repository.GetAll()
                .Select(p => new { Player = p, RankPoints = _rankService.GetRankPoints(p) })
                .OrderByDescending(p => p.RankPoints)
                .Select(p => p.Player)) {
                player.Rank = rank++;
            }

            _repository.Update();

            return result;
        }
    }
}