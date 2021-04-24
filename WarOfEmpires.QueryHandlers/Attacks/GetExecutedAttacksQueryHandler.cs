using System.Collections.Generic;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Attacks;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Formatting;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Attacks {
    [ScopedServiceImplementation(typeof(IQueryHandler<GetExecutedAttacksQuery, IEnumerable<ExecutedAttackViewModel>>))]
    public sealed class GetExecutedAttacksQueryHandler : IQueryHandler<GetExecutedAttacksQuery, IEnumerable<ExecutedAttackViewModel>> {
        private readonly IWarContext _context;
        private readonly EnumFormatter _formatter;

        public GetExecutedAttacksQueryHandler(IWarContext context, EnumFormatter formatter) {
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
                    DefenderSoldierCasualties = (int?)a.Rounds.Where(r => r.IsAggressor).Sum(r => r.Casualties.Sum(c => c.Soldiers)),
                    DefenderMercenaryCasualties = (int?)a.Rounds.Where(r => r.IsAggressor).Sum(r => r.Casualties.Sum(c => c.Mercenaries)),
                    AttackerSoldierCasualties = (int?)a.Rounds.Where(r => !r.IsAggressor).Sum(r => r.Casualties.Sum(c => c.Soldiers)),
                    AttackerMercenaryCasualties = (int?)a.Rounds.Where(r => !r.IsAggressor).Sum(r => r.Casualties.Sum(c => c.Mercenaries)),
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
                    DefenderSoldierCasualties = a.DefenderSoldierCasualties ?? 0,
                    DefenderMercenaryCasualties = a.DefenderMercenaryCasualties ?? 0,
                    AttackerSoldierCasualties = a.AttackerSoldierCasualties ?? 0,
                    AttackerMercenaryCasualties = a.AttackerMercenaryCasualties ?? 0,
                    Result = _formatter.ToString(a.Result)
                })
                .ToList();
        }
    }
}