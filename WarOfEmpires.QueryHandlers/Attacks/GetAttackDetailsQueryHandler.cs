using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Attacks;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;
using WarOfEmpires.Utilities.Formatting;
using System.Data.Entity;

namespace WarOfEmpires.QueryHandlers.Attacks {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetAttackDetailsQueryHandler : IQueryHandler<GetAttackDetailsQuery, AttackDetailsViewModel> {
        private readonly IWarContext _context;
        private readonly ResourcesMap _resourcesMap;
        private readonly EnumFormatter _formatter;

        public GetAttackDetailsQueryHandler(IWarContext context, ResourcesMap resourcesMap, EnumFormatter formatter) {
            _context = context;
            _resourcesMap = resourcesMap;
            _formatter = formatter;
        }

        public AttackDetailsViewModel Execute(GetAttackDetailsQuery query) {
            var attack = _context.Players
                .Include(p => p.ReceivedAttacks.Select(a => a.Rounds.Select(r => r.Casualties)))
                .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.ReceivedAttacks)
                .SingleOrDefault(a => a.Id == query.AttackId);
            var isRead = attack?.IsRead ?? true;

            if (attack == null) {
                attack = _context.Players
                    .Include(p => p.ExecutedAttacks.Select(a => a.Rounds.Select(r => r.Casualties)))
                    .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                    .SelectMany(p => p.ExecutedAttacks)
                    .Single(a => a.Id == query.AttackId);
            }

            return new AttackDetailsViewModel() {
                Id = attack.Id,
                Date = attack.Date,
                Type = _formatter.ToString(attack.Type),
                IsRead = isRead,
                AttackerId = attack.Attacker.Id,
                Attacker = attack.Attacker.DisplayName,
                AttackerAllianceId = attack.Attacker.Alliance?.Id,
                AttackerAllianceCode = attack.Attacker.Alliance?.Code,
                AttackerAllianceName = attack.Attacker.Alliance?.Name,
                DefenderId = attack.Defender.Id,
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