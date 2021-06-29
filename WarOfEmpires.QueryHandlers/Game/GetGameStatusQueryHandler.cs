using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Game;
using WarOfEmpires.Models.Game;
using WarOfEmpires.Queries.Game;
using WarOfEmpires.QueryHandlers.Decorators;

namespace WarOfEmpires.QueryHandlers.Players {
    [TransientServiceImplementation(typeof(IQueryHandler<GetGameStatusQuery, GameStatusViewModel>))]
    public sealed class GetGameStatusQueryHandler : IQueryHandler<GetGameStatusQuery, GameStatusViewModel> {
        private readonly IWarContext _context;

        public GetGameStatusQueryHandler(IWarContext context) {
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
