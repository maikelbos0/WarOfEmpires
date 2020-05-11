using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Attacks;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Formatting;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Attacks {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetExecutedAttacksQueryHandler : IQueryHandler<GetExecutedAttacksQuery, IEnumerable<ExecutedAttackViewModel>> {
        private readonly IWarContext _context;
        private readonly EnumFormatter _formatter;

        public GetExecutedAttacksQueryHandler(IWarContext context, EnumFormatter formatter) {
            _context = context;
            _formatter = formatter;
        }

        public IEnumerable<ExecutedAttackViewModel> Execute(GetExecutedAttacksQuery query) {
            return _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .ExecutedAttacks
                .Select(a => new ExecutedAttackViewModel() {
                    Id = a.Id,
                    Date = a.Date,
                    Turns = a.Turns,
                    Type = _formatter.ToString(a.Type),
                    Defender = a.Defender.DisplayName,
                    DefenderAlliance = a.Defender.Alliance?.Code,
                    DefenderSoldierCasualties = a.Rounds.Where(r => r.IsAggressor).Sum(r => r.Casualties.Sum(c => c.Soldiers)),
                    DefenderMercenaryCasualties = a.Rounds.Where(r => r.IsAggressor).Sum(r => r.Casualties.Sum(c => c.Mercenaries)),
                    AttackerSoldierCasualties = a.Rounds.Where(r => !r.IsAggressor).Sum(r => r.Casualties.Sum(c => c.Soldiers)),
                    AttackerMercenaryCasualties = a.Rounds.Where(r => !r.IsAggressor).Sum(r => r.Casualties.Sum(c => c.Mercenaries)),
                    Result = _formatter.ToString(a.Result)
                })
                .ToList();
        }
    }
}