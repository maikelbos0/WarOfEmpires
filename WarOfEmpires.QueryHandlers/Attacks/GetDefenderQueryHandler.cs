﻿using Microsoft.EntityFrameworkCore;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Game;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Attacks;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Attacks {
    [TransientServiceImplementation(typeof(IQueryHandler<GetDefenderQuery, ExecuteAttackModel>))]
    public sealed class GetDefenderQueryHandler : IQueryHandler<GetDefenderQuery, ExecuteAttackModel> {
        private readonly IWarContext _context;

        public GetDefenderQueryHandler(IWarContext context) {
            _context = context;
        }

        [Audit]
        public ExecuteAttackModel Execute(GetDefenderQuery query) {
            var currentPlayer = _context.Players
                .Include(p => p.Alliance).ThenInclude(a => a.Wars).ThenInclude(w => w.Alliances)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));
            var player = _context.Players
                .Include(p => p.Alliance)
                .Single(p => p.User.Status == UserStatus.Active && p.Id == query.Id);

            return new ExecuteAttackModel() {
                DefenderId = player.Id,
                DisplayName = player.DisplayName,
                Population = player.Peasants + player.Workers.Sum(w => w.Count) + player.Troops.Sum(t => t.GetTotals()),
                Turns = 10,
                // TODO for revenges, war damage will apply for a certain period after getting attacked while at war
                IsAtWar = currentPlayer.Alliance != null && player.Alliance != null && currentPlayer.Alliance.Wars.Any(w => w.Alliances.Contains(player.Alliance)),
                IsTruce = _context.GameStatus.Single().Phase == GamePhase.Truce
            };
        }
    }
}
