using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Attacks;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Attacks {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetAttackDetailsQueryHandler : IQueryHandler<GetAttackDetailsQuery, AttackDetailsViewModel> {
        private readonly IWarContext _context;
        private readonly ResourcesMap _resourcesMap;

        public GetAttackDetailsQueryHandler(IWarContext context, ResourcesMap resourcesMap) {
            _context = context;
            _resourcesMap = resourcesMap;
        }

        public AttackDetailsViewModel Execute(GetAttackDetailsQuery query) {
            var attackId = int.Parse(query.AttackId);
            var attack = _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .ReceivedAttacks.SingleOrDefault(a => a.Id == attackId);
            var isRead = attack?.IsRead ?? true;

            if (attack == null) {
                attack = _context.Players
                    .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                    .ExecutedAttacks.Single(a => a.Id == attackId);
            }

            return new AttackDetailsViewModel() {
                Id = attack.Id,
                Date = attack.Date,
                Type = attack.Type.ToString(),
                IsRead = isRead,
                Attacker = attack.Attacker.DisplayName,
                Defender = attack.Defender.DisplayName,
                Turns = attack.Turns,
                Result = attack.Result.ToString(),
                Resources = _resourcesMap.ToViewModel(attack.Resources),
                Rounds = attack.Rounds.Select(r => new AttackRoundDetailsViewModel() {
                    IsAggressor = r.IsAggressor,
                    Attacker = r.IsAggressor ? attack.Attacker.DisplayName : attack.Defender.DisplayName,
                    Defender = r.IsAggressor ? attack.Defender.DisplayName : attack.Attacker.DisplayName,
                    TroopType = r.TroopType.ToString(),
                    Troops = r.Troops,
                    Damage = r.Damage,
                    SoldierCasualties = r.Casualties.Sum(c => c.Soldiers),
                    MercenaryCasualties = r.Casualties.Sum(c => c.Mercenaries)
                }).ToList()
            };
        }
    }
}