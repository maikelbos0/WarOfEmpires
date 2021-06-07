using Microsoft.EntityFrameworkCore;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Attacks;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Formatting;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Attacks {
    [TransientServiceImplementation(typeof(IQueryHandler<GetAttackDetailsQuery, AttackDetailsViewModel>))]
    public sealed class GetAttackDetailsQueryHandler : IQueryHandler<GetAttackDetailsQuery, AttackDetailsViewModel> {
        private readonly IWarContext _context;
        private readonly IResourcesMap _resourcesMap;
        private readonly IEnumFormatter _formatter;

        public GetAttackDetailsQueryHandler(IWarContext context, IResourcesMap resourcesMap, IEnumFormatter formatter) {
            _context = context;
            _resourcesMap = resourcesMap;
            _formatter = formatter;
        }

        [Audit]
        public AttackDetailsViewModel Execute(GetAttackDetailsQuery query) {
            var attack = _context.Players
                .Include(p => p.ReceivedAttacks).ThenInclude(a => a.Rounds).ThenInclude(r => r.Casualties)
                .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.ReceivedAttacks)
                .SingleOrDefault(a => a.Id == query.AttackId);
            var isRead = attack?.IsRead ?? true;

            if (attack == null) {
                attack = _context.Players
                    .Include(p => p.ExecutedAttacks).ThenInclude(a => a.Rounds).ThenInclude(r => r.Casualties)
                    .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                    .SelectMany(p => p.ExecutedAttacks)
                    .Single(a => a.Id == query.AttackId);
            }

            return new AttackDetailsViewModel() {
                Id = attack.Id,
                Date = attack.Date,
                Type = _formatter.ToString(attack.Type),
                IsRead = isRead,
                AttackerId = attack.Attacker.User.Status == UserStatus.Active ? attack.Attacker.Id : default(int?),
                Attacker = attack.Attacker.DisplayName,
                AttackerAllianceId = attack.Attacker.Alliance?.Id,
                AttackerAllianceCode = attack.Attacker.Alliance?.Code,
                AttackerAllianceName = attack.Attacker.Alliance?.Name,
                DefenderId = attack.Defender.User.Status == UserStatus.Active ? attack.Defender.Id : default(int?),
                Defender = attack.Defender.DisplayName,
                DefenderAllianceId = attack.Defender.Alliance?.Id,
                DefenderAllianceCode = attack.Defender.Alliance?.Code,
                DefenderAllianceName = attack.Defender.Alliance?.Name,
                Turns = attack.Turns,
                Result = _formatter.ToString(attack.Result),
                Resources = _resourcesMap.ToViewModel(attack.Resources),
                Rounds = attack.Rounds.Select(r => new AttackRoundDetailsViewModel() {
                    IsAggressor = r.IsAggressor,
                    Attacker = r.IsAggressor ? attack.Attacker.DisplayName : attack.Defender.DisplayName,
                    Defender = r.IsAggressor ? attack.Defender.DisplayName : attack.Attacker.DisplayName,
                    TroopType = _formatter.ToString(r.TroopType),
                    Troops = r.Troops,
                    Damage = r.Damage,
                    SoldierCasualties = r.Casualties.Sum(c => c.Soldiers),
                    MercenaryCasualties = r.Casualties.Sum(c => c.Mercenaries)
                }).ToList()
            };
        }
    }
}