using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Players {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetNotificationsQueryHandler : IQueryHandler<GetNotificationsQuery, NotificationsViewModel> {
        private readonly IWarContext _context;

        public GetNotificationsQueryHandler(IWarContext context) {
            _context = context;
        }

        public NotificationsViewModel Execute(GetNotificationsQuery query) {
            var player = _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            return new NotificationsViewModel() {
                HasNewMessages = player.ReceivedMessages.Any(m => !m.IsRead),
                HasNewAttacks = player.ReceivedAttacks.Any(a => !a.IsRead),
                HasNewMarketSales = player.HasNewMarketSales,
                HasHousingShortage = player.GetTheoreticalRecruitsPerDay() > player.GetAvailableHousingCapacity(),
                HasUpkeepShortage = player.HasUpkeepRunOut || player.WillUpkeepRunOut(),
                HasSoldierShortage = player.GetSoldierRecruitsPenalty() > 0,
                HasNewInvites = player.Invites.Any(i => !i.IsRead)
            };
        }
    }
}