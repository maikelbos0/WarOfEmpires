using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Attacks;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.Utilities.Auditing;
using WarOfEmpires.Utilities.Formatting;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Attacks {
    public sealed class GetExecutedAttacksQueryHandler : IQueryHandler<GetExecutedAttacksQuery, IEnumerable<ExecutedAttackViewModel>> {
        private readonly IReadOnlyWarContext _context;
        private readonly IEnumFormatter _formatter;

        public GetExecutedAttacksQueryHandler(IReadOnlyWarContext context, IEnumFormatter formatter) {
            _context = context;
            _formatter = formatter;
        }

        [Audit]
        public IEnumerable<ExecutedAttackViewModel> Execute(GetExecutedAttacksQuery query) {
            return _context.Players
                .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.ExecutedAttacks)
                .Select(a => new {
                    a.Id,
                    a.Date,
                    a.Turns,
                    a.Type,
                    Defender = a.Defender.DisplayName,
                    DefenderAlliance = a.Defender.Alliance == null ? null : a.Defender.Alliance.Code,
                    Casualties = a.Rounds.SelectMany(r => r.Casualties.Select(c => new {
                        r.IsAggressor,
                        c.Soldiers,
                        c.Mercenaries
                    })),
                    a.Result
                })
                .ToList()
                .Select(a => new ExecutedAttackViewModel() {
                    Id = a.Id,
                    Date = a.Date,
                    Turns = a.Turns,
                    Type = _formatter.ToString(a.Type),
                    Defender = a.Defender,
                    DefenderAlliance = a.DefenderAlliance,
                    DefenderSoldierCasualties = a.Casualties.Where(c => c.IsAggressor).Sum(c => c.Soldiers),
                    DefenderMercenaryCasualties = a.Casualties.Where(c => c.IsAggressor).Sum(c => c.Mercenaries),
                    AttackerSoldierCasualties = a.Casualties.Where(c => !c.IsAggressor).Sum(c => c.Soldiers),
                    AttackerMercenaryCasualties = a.Casualties.Where(c => !c.IsAggressor).Sum(c => c.Mercenaries),
                    Result = _formatter.ToString(a.Result)
                })
                .ToList();
        }
    }
}