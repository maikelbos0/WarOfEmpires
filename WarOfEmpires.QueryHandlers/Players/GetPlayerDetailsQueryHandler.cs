﻿using System.Data.Entity;
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
                    CanBeAttacked = p.Id != currentPlayer.Id && (p.Alliance == null || (p.Alliance.Id != currentAllianceId && !p.Alliance.NonAggressionPacts.Any(pact => pact.Alliances.Any(pa => pa.Id == currentAllianceId))))
                })
                .Single();

            return new PlayerDetailsViewModel() {
                Id = player.Id,
                Rank = player.Rank,
                Title = _formatter.ToString(player.Title),
                DisplayName = player.DisplayName,
                Population = player.Population,
                AllianceId = player.Alliance?.Id,
                AllianceCode = player.Alliance?.Code,
                AllianceName = player.Alliance?.Name,
                CanBeAttacked = player.CanBeAttacked
            };
        }
    }
}