using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Formatting;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Players {
    [TransientServiceImplementation(typeof(IQueryHandler<GetPlayersQuery, IEnumerable<PlayerViewModel>>))]
    public sealed class GetPlayersQueryHandler : IQueryHandler<GetPlayersQuery, IEnumerable<PlayerViewModel>> {
        private readonly IWarContext _context;
        private readonly IEnumFormatter _formatter;

        public GetPlayersQueryHandler(IWarContext context, IEnumFormatter formatter) {
            _context = context;
            _formatter = formatter;
        }

        [Audit]
        public IEnumerable<PlayerViewModel> Execute(GetPlayersQuery query) {
            var currentPlayer = _context.Players
                .Include(p => p.Alliance)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));
            var currentAllianceId = currentPlayer.Alliance?.Id;
            var players = _context.Players
                .Where(p => p.User.Status == UserStatus.Active);

            if (!string.IsNullOrEmpty(query.DisplayName)) {
                players = players.Where(p => p.DisplayName.Contains(query.DisplayName));
            }

            // Materialize before setting the title
            return players
                .Select(p => new {
                    p.Id,
                    Status = p.Id == currentPlayer.Id ? "Mine" : p.Alliance != null ? (p.Alliance.Id == currentAllianceId ? "Ally" : p.Alliance.NonAggressionPacts.Any(pact => pact.Alliances.Any(pa => pa.Id == currentAllianceId)) ? "Pact" : null) : null,
                    p.Rank,
                    p.Title,
                    p.DisplayName,
                    Population = p.Peasants
                        + (p.Workers.Sum(w => (int?)w.Count) ?? 0)
                        + (p.Troops.Sum(t => (int?)t.Soldiers) ?? 0)
                        + (p.Troops.Sum(t => (int?)t.Mercenaries) ?? 0),
                    p.Alliance
                })
                .ToList()
                .Select(p => new PlayerViewModel() {
                    Id = p.Id,
                    Status = p.Status,
                    Rank = p.Rank,
                    Title = _formatter.ToString(p.Title),
                    DisplayName = p.DisplayName,
                    Population = p.Population,
                    Alliance = p.Alliance?.Code
                });
        }
    }
}