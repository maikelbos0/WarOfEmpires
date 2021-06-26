using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Decorators;

namespace WarOfEmpires.QueryHandlers.Players {
    [TransientServiceImplementation(typeof(IQueryHandler<GetVictoryStatusQuery, VictoryStatusViewModel>))]
    public sealed class GetVictoryStatusQueryHandler : IQueryHandler<GetVictoryStatusQuery, VictoryStatusViewModel> {
        private readonly IWarContext _context;

        public GetVictoryStatusQueryHandler(IWarContext context) {
            _context = context;
        }

        [Audit]
        public VictoryStatusViewModel Execute(GetVictoryStatusQuery query) {
            var grandOverlord = _context.Players.SingleOrDefault(p => p.Title == TitleType.GrandOverlord && p.User.Status == UserStatus.Active);

            return new VictoryStatusViewModel() {
                CurrentGrandOverlordId = grandOverlord?.Id,
                CurrentGrandOverlord = grandOverlord?.DisplayName,
                GrandOverlordTime = grandOverlord?.GrandOverlordTime
            };
        }
    }
}
