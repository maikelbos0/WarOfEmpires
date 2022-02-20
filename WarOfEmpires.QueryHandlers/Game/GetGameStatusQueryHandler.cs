using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Game;
using WarOfEmpires.Models.Game;
using WarOfEmpires.Queries.Game;
using WarOfEmpires.QueryHandlers.Decorators;

namespace WarOfEmpires.QueryHandlers.Players {
    public sealed class GetGameStatusQueryHandler : IQueryHandler<GetGameStatusQuery, GameStatusViewModel> {
        private readonly IReadOnlyWarContext _context;

        public GetGameStatusQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        [Audit]
        public GameStatusViewModel Execute(GetGameStatusQuery query) {
            return _context.GameStatus.Select(s => new GameStatusViewModel() {
                CurrentGrandOverlordId = s.CurrentGrandOverlord.Id,
                CurrentGrandOverlord = s.CurrentGrandOverlord.DisplayName,
                CurrentGrandOverlordTime = s.CurrentGrandOverlord.GrandOverlordTime,
                Phase = s.Phase.ToString(),
                GrandOverlordHoursToWin = GameStatus.GrandOverlordHoursToWin
            }).Single();
        }
    }
}
