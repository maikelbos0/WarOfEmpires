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
    public sealed class GetExecutedAttacksQueryHandler : IQueryHandler<GetExecutedAttacksQuery, List<ExecutedAttackViewModel>> {
        private readonly IWarContext _context;

        public GetExecutedAttacksQueryHandler(IWarContext context) {
            _context = context;
        }

        public List<ExecutedAttackViewModel> Execute(GetExecutedAttacksQuery query) {
            return _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .ExecutedAttacks
                .Select(a => new ExecutedAttackViewModel() {
                    Id = a.Id,
                    Date = a.Date,
                    Turns = a.Turns,
                    Defender = a.Defender.DisplayName,
                    DefenderSoldierCasualties = a.Rounds.Where(r => r.IsAggressor).Sum(r => r.Casualties.Archers.Soldiers + r.Casualties.Cavalry.Soldiers + r.Casualties.Footmen.Soldiers),
                    DefenderMercenaryCasualties = a.Rounds.Where(r => r.IsAggressor).Sum(r => r.Casualties.Archers.Mercenaries + r.Casualties.Cavalry.Mercenaries + r.Casualties.Footmen.Mercenaries),
                    AttackerSoldierCasualties = a.Rounds.Where(r => !r.IsAggressor).Sum(r => r.Casualties.Archers.Soldiers + r.Casualties.Cavalry.Soldiers + r.Casualties.Footmen.Soldiers),
                    AttackerMercenaryCasualties = a.Rounds.Where(r => !r.IsAggressor).Sum(r => r.Casualties.Archers.Mercenaries + r.Casualties.Cavalry.Mercenaries + r.Casualties.Footmen.Mercenaries),
                    Result = a.Result.ToString()
                })
                .ToList();
        }
    }
}