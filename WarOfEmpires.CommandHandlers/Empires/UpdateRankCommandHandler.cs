using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Game;

namespace WarOfEmpires.CommandHandlers.Empires {
    [TransientServiceImplementation(typeof(ICommandHandler<UpdateRankCommand>))]
    public sealed class UpdateRankCommandHandler : ICommandHandler<UpdateRankCommand> {
        private readonly IPlayerRepository _repository;
        private readonly IRankService _rankService;
        private readonly IGameStatus _gameStatus;

        public UpdateRankCommandHandler(IPlayerRepository repository, IRankService rankService, IGameStatus gameStatus) {
            _repository = repository;
            _rankService = rankService;
            _gameStatus = gameStatus;
        }

        [Audit]
        public CommandResult<UpdateRankCommand> Execute(UpdateRankCommand command) {
            var result = new CommandResult<UpdateRankCommand>();
            var players = _repository.GetAll();

            _rankService.Update(players);
            _repository.SaveChanges();

            var grandOverlord = players.SingleOrDefault(p => p.Title == TitleType.GrandOverlord);

            if (grandOverlord == null) {
                _gameStatus.CurrentGrandOverlordId = null;
                _gameStatus.CurrentGrandOverlord = null;
                _gameStatus.CurrentGrandOverlordTime = null;
            }
            else {
                _gameStatus.CurrentGrandOverlordId = grandOverlord.Id;
                _gameStatus.CurrentGrandOverlord = grandOverlord.DisplayName;
                _gameStatus.CurrentGrandOverlordTime = grandOverlord.GrandOverlordTime;
            }

            // TODO create and dispatch event for truce here if game won

            return result;
        }
    }
}