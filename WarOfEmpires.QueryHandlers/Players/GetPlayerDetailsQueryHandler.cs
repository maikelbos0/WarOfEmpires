using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.Utilities.Auditing;
using WarOfEmpires.Utilities.Configuration;
using WarOfEmpires.Utilities.Formatting;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Players {
    public sealed class GetPlayerDetailsQueryHandler : IQueryHandler<GetPlayerDetailsQuery, PlayerDetailsViewModel> {
        private readonly IReadOnlyWarContext _context;
        private readonly IEnumFormatter _formatter;
        private readonly AppSettings _appSettings;

        public GetPlayerDetailsQueryHandler(IReadOnlyWarContext context, IEnumFormatter formatter, AppSettings appSettings) {
            _formatter = formatter;
            _context = context;
            _appSettings = appSettings;
        }

        [Audit]
        public PlayerDetailsViewModel Execute(GetPlayerDetailsQuery query) {
            var currentPlayer = _context.Players
                .Include(p => p.Alliance).ThenInclude(a => a.Wars).ThenInclude(w => w.Alliances)
                .Include(p => p.Alliance).ThenInclude(a => a.NonAggressionPacts).ThenInclude(p => p.Alliances)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));
            var currentAllianceId = currentPlayer.Alliance?.Id;

            // Materialize before setting the title and race
            var player = _context.Players
                .Where(p => p.User.Status == UserStatus.Active && p.Id == query.Id)
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
                    Defences = p.Buildings.SingleOrDefault(b => b.Type == BuildingType.Defences),
                    p.Alliance,
                    p.GrandOverlordTime,
                    CanBeAttacked = p.Id != currentPlayer.Id && p.CreationDate <= DateTime.UtcNow.AddHours(-Player.NewPlayerTruceHours) && (p.Alliance == null || (p.Alliance.Id != currentAllianceId && !p.Alliance.NonAggressionPacts.Any(pact => pact.Alliances.Any(pa => pa.Id == currentAllianceId)))),
                    p.CreationDate,
                    p.Profile
                })
                .Single();

            return new PlayerDetailsViewModel() {
                Id = player.Id,
                Status = GetStatus(currentPlayer, player.Id, player.Alliance, player.CreationDate),
                Rank = player.Rank,
                Title = _formatter.ToString(player.Title),
                DisplayName = player.DisplayName,
                Race = _formatter.ToString(player.Race),
                Population = player.Population,
                Defences = BuildingDefinitionFactory.Get(BuildingType.Defences).GetName(player.Defences?.Level ?? 0),
                AllianceId = player.Alliance?.Id,
                AllianceCode = player.Alliance?.Code,
                AllianceName = player.Alliance?.Name,
                CanBeAttacked = player.CanBeAttacked,
                GrandOverlordTime = player.GrandOverlordTime == TimeSpan.Zero ? null : player.GrandOverlordTime,
                FullName = player.Profile?.FullName,
                Description = player.Profile?.Description,
                AvatarLocation = string.IsNullOrWhiteSpace(player.Profile?.AvatarLocation) ? null : $"{_appSettings.UserImageBaseUrl?.TrimEnd('/')}/{player.Profile.AvatarLocation}"
            };
        }

        private string GetStatus(Player currentPlayer, int id, Alliance alliance, DateTime creationDate) {
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