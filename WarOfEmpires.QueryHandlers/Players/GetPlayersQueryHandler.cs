using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.Utilities.Formatting;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Players {
    public sealed class GetPlayersQueryHandler : IQueryHandler<GetPlayersQuery, IEnumerable<PlayerViewModel>> {
        private readonly IReadOnlyWarContext _context;
        private readonly IEnumFormatter _formatter;

        public GetPlayersQueryHandler(IReadOnlyWarContext context, IEnumFormatter formatter) {
            _context = context;
            _formatter = formatter;
        }

        public IEnumerable<PlayerViewModel> Execute(GetPlayersQuery query) {
            var currentPlayer = _context.Players
                .Include(p => p.Alliance).ThenInclude(a => a.Wars).ThenInclude(w => w.Alliances)
                .Include(p => p.Alliance).ThenInclude(a => a.NonAggressionPacts).ThenInclude(p => p.Alliances)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));
            var currentAllianceId = currentPlayer.Alliance?.Id;
            var players = _context.Players
                .Where(p => p.User.Status == UserStatus.Active);

            if (!string.IsNullOrEmpty(query.DisplayName)) {
                players = players.Where(p => p.DisplayName.Contains(query.DisplayName));
            }

            // Materialize before setting the title, status and race
            return players
                .Select(p => new {
                    p.Id,
                    p.Rank,
                    p.Title,
                    p.DisplayName,
                    p.Race,
                    Population = p.Peasants
                        + (p.Workers.Sum(w => (int?)w.Count) ?? 0)
                        + (p.Troops.Sum(t => (int?)t.Soldiers) ?? 0)
                        + (p.Troops.Sum(t => (int?)t.Mercenaries) ?? 0),
                    p.Alliance,
                    p.GrandOverlordTime,
                    p.CreationDate
                })
                .ToList()
                .Select(p => new PlayerViewModel() {
                    Id = p.Id,
                    Status = GetStatus(currentPlayer, p.Id, p.Alliance, p.CreationDate),
                    Rank = p.Rank,
                    Title = _formatter.ToString(p.Title),
                    DisplayName = p.DisplayName,
                    Race = _formatter.ToString(p.Race),
                    Population = p.Population,
                    Alliance = p.Alliance?.Code,
                    GrandOverlordTime = p.GrandOverlordTime
                });
        }

        private static string GetStatus(Player currentPlayer, int id, Alliance alliance, DateTime creationDate) {
            if (currentPlayer.Id == id) {
                return "Mine";
            }
            else if (creationDate > DateTime.UtcNow.AddHours(-Player.NewPlayerTruceHours)) {
                return "New";
            }
            else if (currentPlayer.Alliance != null && alliance != null) {
                if (currentPlayer.Alliance == alliance) {
                    return "Ally";
                }
                else if (currentPlayer.Alliance.NonAggressionPacts.Any(p => p.Alliances.Any(a => a == alliance))) {
                    return "Pact";
                }
                else if (currentPlayer.Alliance.Wars.Any(w => w.Alliances.Any(a => a == alliance))) {
                    return "War";
                }
            }

            return null;
        }
    }
}