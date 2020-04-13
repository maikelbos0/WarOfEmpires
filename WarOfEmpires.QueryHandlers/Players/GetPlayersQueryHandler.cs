using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Players {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetPlayersQueryHandler : IQueryHandler<GetPlayersQuery, IEnumerable<PlayerViewModel>> {
        private readonly IWarContext _context;

        public GetPlayersQueryHandler(IWarContext context) {
            _context = context;
        }

        public IEnumerable<PlayerViewModel> Execute(GetPlayersQuery query) {
            var players = _context.Players
                .Where(p => p.User.Status == UserStatus.Active);

            if (!string.IsNullOrEmpty(query.DisplayName)) {
                players = players.Where(p => p.DisplayName.Contains(query.DisplayName));
            }

            return players
                .Select(p => new PlayerViewModel() {
                    Id = p.Id,
                    DisplayName = p.DisplayName,
                    Population = p.Peasants 
                        + (p.Workers.Sum(w => (int?)w.Count) ?? 0)
                        + (p.Troops.Sum(t => (int?)t.Soldiers) ?? 0)
                        + (p.Troops.Sum(t => (int?)t.Mercenaries) ?? 0)
                })
                .ToList();
        }
    }
}