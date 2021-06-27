using VDT.Core.DependencyInjection;
using WarOfEmpires.Models.Game;
using WarOfEmpires.Queries.Game;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Game;

namespace WarOfEmpires.QueryHandlers.Players {
    [TransientServiceImplementation(typeof(IQueryHandler<GetGameStatusQuery, GameStatusViewModel>))]
    public sealed class GetGameStatusQueryHandler : IQueryHandler<GetGameStatusQuery, GameStatusViewModel> {
        private readonly IGameStatus _gameStatus;

        public GetGameStatusQueryHandler(IGameStatus gameStatus) {
            _gameStatus = gameStatus;
        }

        [Audit]
        public GameStatusViewModel Execute(GetGameStatusQuery query) {
            return new GameStatusViewModel() {
                CurrentGrandOverlordId = _gameStatus.CurrentGrandOverlordId,
                CurrentGrandOverlord = _gameStatus.CurrentGrandOverlord,
                CurrentGrandOverlordTime = _gameStatus.CurrentGrandOverlordTime
            };
        }
    }
}
