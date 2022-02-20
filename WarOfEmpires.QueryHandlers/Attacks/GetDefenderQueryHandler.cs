using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Game;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Attacks;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Attacks {
    public sealed class GetDefenderQueryHandler : IQueryHandler<GetDefenderQuery, ExecuteAttackModel> {
        private readonly IReadOnlyWarContext _context;

        public GetDefenderQueryHandler(IReadOnlyWarContext context) {
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
            var validAttackTypes = new List<string>() { AttackType.Assault.ToString(), AttackType.Raid.ToString() };
            var revengeExpirationCutoff = DateTime.UtcNow.AddHours(-Attack.RevengeExpirationHours);
            var lastExecutedRevengeDate = currentPlayer.ExecutedAttacks
                .Where(a => a.Defender == player && a.Type == AttackType.Revenge)
                .DefaultIfEmpty(null)
                .Max(a => a?.Date);

            if (lastExecutedRevengeDate.HasValue && lastExecutedRevengeDate.Value > revengeExpirationCutoff) {
                revengeExpirationCutoff = lastExecutedRevengeDate.Value;
            }

            if (currentPlayer.ReceivedAttacks.Any(a => a.Attacker == player && a.Date >= revengeExpirationCutoff)) {
                validAttackTypes.Add(AttackType.Revenge.ToString());
            }

            if (player.Title == TitleType.GrandOverlord && currentPlayer.Title == TitleType.Overlord) {
                validAttackTypes.Add(AttackType.GrandOverlordAttack.ToString());
            }

            return new ExecuteAttackModel() {
                DefenderId = player.Id,
                Defender = player.DisplayName,
                Population = player.Peasants + player.Workers.Sum(w => w.Count) + player.Troops.Sum(t => t.GetTotals()),
                Turns = 10,
                HasWarDamage = (currentPlayer.Alliance != null && player.Alliance != null && currentPlayer.Alliance.Wars.Any(w => w.Alliances.Contains(player.Alliance)))
                    || currentPlayer.ReceivedAttacks.Any(a => a.IsAtWar && a.Attacker == player && a.Date >= DateTime.UtcNow.AddHours(-Attack.WarAttackExpirationHours)),
                IsTruce = _context.GameStatus.Single().Phase == GamePhase.Truce,
                ValidAttackTypes = validAttackTypes
            };
        }
    }
}
