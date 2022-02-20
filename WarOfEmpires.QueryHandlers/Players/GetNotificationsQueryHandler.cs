using Microsoft.EntityFrameworkCore;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Players {
    public sealed class GetNotificationsQueryHandler : IQueryHandler<GetNotificationsQuery, NotificationsViewModel> {
        private readonly IReadOnlyWarContext _context;

        public GetNotificationsQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        [Audit]
        public NotificationsViewModel Execute(GetNotificationsQuery query) {
            var player = _context.Players
                .Include(p => p.Workers)
                .Include(p => p.Troops)
                .Include(p => p.Buildings)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            return new NotificationsViewModel() {
                HasNewMessages = player.ReceivedMessages.Any(m => !m.IsRead),
                HasNewAttacks = player.ReceivedAttacks.Any(a => !a.IsRead),
                HasNewMarketSales = player.HasNewMarketSales,
                HasNewChatMessages = player.HasNewChatMessages,
                HasHousingShortage = player.GetTheoreticalRecruitsPerDay() > player.GetAvailableHousingCapacity(),
                HasUpkeepShortage = player.HasUpkeepRunOut || player.WillUpkeepRunOut(),
                HasSoldierShortage = player.GetSoldierRecruitsPenalty() > 0,
                HasNewInvites = player.Invites.Any(i => !i.IsRead)
            };
        }
    }
}