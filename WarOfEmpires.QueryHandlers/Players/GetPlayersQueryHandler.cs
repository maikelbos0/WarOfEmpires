using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Players {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetPlayersQueryHandler : IQueryHandler<GetPlayersQuery, IEnumerable<PlayerViewModel>> {
        private readonly IWarContext _context;
        private readonly EnumFormatter _formatter;

        public GetPlayersQueryHandler(IWarContext context, EnumFormatter formatter) {
            _context = context;
            _formatter = formatter;
        }

        public IEnumerable<PlayerViewModel> Execute(GetPlayersQuery query) {
            var players = _context.Players
                .Where(p => p.User.Status == UserStatus.Active);

            if (!string.IsNullOrEmpty(query.DisplayName)) {
                players = players.Where(p => p.DisplayName.Contains(query.DisplayName));
            }

            // Materialize before setting the title
            return players
                .Select(p => new {
                    p.Id,
                    p.Rank,
                    p.Title,
                    p.DisplayName,
                    Population = p.Peasants
                        + (p.Workers.Sum(w => (int?)w.Count) ?? 0)
                        + (p.Troops.Sum(t => (int?)t.Soldiers) ?? 0)
                        + (p.Troops.Sum(t => (int?)t.Mercenaries) ?? 0),
                    Alliance = p.Alliance.Code
                })
                .ToList()
                .Select(p => new PlayerViewModel() {
                    Id = p.Id,
                    Rank = p.Rank,
                    Title = _formatter.ToString(p.Title),
                    DisplayName = p.DisplayName,
                    Population = p.Population,
                    Alliance = p.Alliance
                });
        }
    }
}