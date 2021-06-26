using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Formatting;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Players {
    [TransientServiceImplementation(typeof(IQueryHandler<GetPlayerDetailsQuery, PlayerDetailsViewModel>))]
    public sealed class GetPlayerDetailsQueryHandler : IQueryHandler<GetPlayerDetailsQuery, PlayerDetailsViewModel> {
        private readonly IWarContext _context;
        private readonly IEnumFormatter _formatter;

        public GetPlayerDetailsQueryHandler(IWarContext context, IEnumFormatter formatter) {
            _formatter = formatter;
            _context = context;
        }

        [Audit]
        public PlayerDetailsViewModel Execute(GetPlayerDetailsQuery query) {
            var currentPlayer = _context.Players
                .Include(p => p.Alliance).ThenInclude(a => a.Wars).ThenInclude(w => w.Alliances)
                .Include(p => p.Alliance).ThenInclude(a => a.NonAggressionPacts).ThenInclude(p => p.Alliances)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));
            var currentAllianceId = currentPlayer.Alliance?.Id;
            
            // Materialize before setting the title
            var player = _context.Players
                .Where(p => p.User.Status == UserStatus.Active && p.Id == query.Id)
                .Select( p => new {
                    p.Id,
                    p.Rank,
                    p.Title,
                    p.DisplayName,
                    Population = p.Peasants
                        + (p.Workers.Sum(w => (int?)w.Count) ?? 0)
                        + (p.Troops.Sum(t => (int?)t.Soldiers) ?? 0)
                        + (p.Troops.Sum(t => (int?)t.Mercenaries) ?? 0),
                    p.Alliance,
                    p.GrandOverlordTime,
                    CanBeAttacked = p.Id != currentPlayer.Id && (p.Alliance == null || (p.Alliance.Id != currentAllianceId && !p.Alliance.NonAggressionPacts.Any(pact => pact.Alliances.Any(pa => pa.Id == currentAllianceId))))
                })
                .Single();

            return new PlayerDetailsViewModel() {
                Id = player.Id,
                Status = GetStatus(currentPlayer, player.Id, player.Alliance),
                Rank = player.Rank,
                Title = _formatter.ToString(player.Title),
                DisplayName = player.DisplayName,
                Population = player.Population,
                AllianceId = player.Alliance?.Id,
                AllianceCode = player.Alliance?.Code,
                AllianceName = player.Alliance?.Name,
                CanBeAttacked = player.CanBeAttacked,
                GrandOverlordTime = player.GrandOverlordTime == TimeSpan.Zero ? null : player.GrandOverlordTime
            };
        }

        public string GetStatus(Player currentPlayer, int id, Alliance alliance) {
            if (currentPlayer.Id == id) {
                return "Mine";
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