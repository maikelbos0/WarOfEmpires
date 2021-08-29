using Microsoft.EntityFrameworkCore;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Players {
    [TransientServiceImplementation(typeof(IQueryHandler<GetPlayerHomeQuery, PlayerHomeViewModel>))]
    public sealed class GetPlayerHomeQueryHandler : IQueryHandler<GetPlayerHomeQuery, PlayerHomeViewModel> {
        private readonly IWarContext _context;

        public GetPlayerHomeQueryHandler(IWarContext context) {
            _context = context;
        }

        [Audit]
        public PlayerHomeViewModel Execute(GetPlayerHomeQuery query) {
            var player = _context.Players
                .Include(p => p.Workers)
                .Include(p => p.Troops)
                .Include(p => p.Buildings)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            return new PlayerHomeViewModel() {
                DisplayName = player.DisplayName,
                HasNewMessages = player.ReceivedMessages.Any(m => !m.IsRead),
                NewAttackCount = player.ReceivedAttacks.Count(a => !a.IsRead),
                TotalSoldierCasualties = player.ReceivedAttacks.Where(a => !a.IsRead).SelectMany(a => a.Rounds).Where(r => r.IsAggressor).Sum(r => r.Casualties.Sum(c => c.Soldiers)),
                TotalMercenaryCasualties = player.ReceivedAttacks.Where(a => !a.IsRead).SelectMany(a => a.Rounds).Where(r => r.IsAggressor).Sum(r => r.Casualties.Sum(c => c.Mercenaries)),
                HasNewMarketSales = player.HasNewMarketSales,
                HasNewChatMessages = player.HasNewChatMessages,
                HasHousingShortage = player.GetTheoreticalRecruitsPerDay() > player.GetAvailableHousingCapacity(),
                HasUpkeepRunOut = player.HasUpkeepRunOut,
                WillUpkeepRunOut = player.WillUpkeepRunOut(),
                HasSoldierShortage = player.GetSoldierRecruitsPenalty() > 0,
                HasNewInvites = player.Invites.Any(i => !i.IsRead),
                CurrentPeasants = player.Peasants
            };
        }
    }
}
