using System.Data.Entity;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Formatting;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Players {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetPlayerDetailsQueryHandler : IQueryHandler<GetPlayerDetailsQuery, PlayerDetailsViewModel> {
        private readonly IWarContext _context;
        private readonly EnumFormatter _formatter;

        public GetPlayerDetailsQueryHandler(IWarContext context, EnumFormatter formatter) {
            _formatter = formatter;
            _context = context;
        }

        public PlayerDetailsViewModel Execute(GetPlayerDetailsQuery query) {
            var currentPlayer = _context.Players
                .Include(p => p.Alliance)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));
            var player = _context.Players
                .Include(p => p.Alliance)
                .Include(p => p.Workers)
                .Include(p => p.Troops)
                .Single(p => p.User.Status == UserStatus.Active && p.Id == query.Id);

            return new PlayerDetailsViewModel() {
                Id = player.Id,
                Rank = player.Rank,
                Title = _formatter.ToString(player.Title),
                DisplayName = player.DisplayName,
                Population = player.Peasants + player.Workers.Sum(w => w.Count) + player.Troops.Sum(t => t.GetTotals()),
                AllianceId = player.Alliance?.Id,
                AllianceCode = player.Alliance?.Code,
                AllianceName = player.Alliance?.Name,
                CanBeAttacked = player != currentPlayer && (player.Alliance == null || player.Alliance != currentPlayer.Alliance)
            };
        }
    }
}