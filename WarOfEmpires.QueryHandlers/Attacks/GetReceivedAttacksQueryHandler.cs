using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Attacks;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Attacks {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetReceivedAttacksQueryHandler : IQueryHandler<GetReceivedAttacksQuery, List<ReceivedAttackViewModel>> {
        private readonly IWarContext _context;

        public GetReceivedAttacksQueryHandler(IWarContext context) {
            _context = context;
        }

        public List<ReceivedAttackViewModel> Execute(GetReceivedAttacksQuery query) {
            return _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .ReceivedAttacks
                .Select(a => new ReceivedAttackViewModel() {
                    Id = a.Id,
                    Date = a.Date,
                    Turns = a.Turns,
                    Attacker = a.Attacker.DisplayName,
                    DefenderSoldierCasualties = a.Rounds.Where(r => r.IsAggressor).Sum(r => r.Casualties.Archers.Soldiers + r.Casualties.Cavalry.Soldiers + r.Casualties.Footmen.Soldiers),
                    DefenderMercenaryCasualties = a.Rounds.Where(r => r.IsAggressor).Sum(r => r.Casualties.Archers.Mercenaries + r.Casualties.Cavalry.Mercenaries + r.Casualties.Footmen.Mercenaries),
                    AttackerSoldierCasualties = a.Rounds.Where(r => !r.IsAggressor).Sum(r => r.Casualties.Archers.Soldiers + r.Casualties.Cavalry.Soldiers + r.Casualties.Footmen.Soldiers),
                    AttackerMercenaryCasualties = a.Rounds.Where(r => !r.IsAggressor).Sum(r => r.Casualties.Archers.Mercenaries + r.Casualties.Cavalry.Mercenaries + r.Casualties.Footmen.Mercenaries),
                    Result = a.Result.ToString(),
                    IsRead = a.IsRead
                })
                .ToList();
        }
    }
}