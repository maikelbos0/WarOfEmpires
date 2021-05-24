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
    [TransientServiceImplementation(typeof(IQueryHandler<GetReceivedAttacksQuery, IEnumerable<ReceivedAttackViewModel>>))]
    public sealed class GetReceivedAttacksQueryHandler : IQueryHandler<GetReceivedAttacksQuery, IEnumerable<ReceivedAttackViewModel>> {
        private readonly IWarContext _context;
        private readonly IEnumFormatter _formatter;

        public GetReceivedAttacksQueryHandler(IWarContext context, IEnumFormatter formatter) {
            _context = context;
            _formatter = formatter;
        }

        [Audit]
        public IEnumerable<ReceivedAttackViewModel> Execute(GetReceivedAttacksQuery query) {
            return _context.Players
                .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.ReceivedAttacks)
                .Select(a => new {
                    a.Id,
                    a.Date,
                    a.Turns,
                    a.Type,
                    Attacker = a.Attacker.DisplayName,
                    AttackerAlliance = a.Attacker.Alliance == null ? null : a.Attacker.Alliance.Code,
                    Casualties = a.Rounds.SelectMany(r => r.Casualties.Select(c => new {
                        r.IsAggressor,
                        c.Soldiers,
                        c.Mercenaries
                    })),
                    a.Result,
                    a.IsRead
                })
                .ToList()
                .Select(a => new ReceivedAttackViewModel() {
                    Id = a.Id,
                    Date = a.Date,
                    Turns = a.Turns,
                    Type = _formatter.ToString(a.Type),
                    Attacker = a.Attacker,
                    AttackerAlliance = a.AttackerAlliance,
                    DefenderSoldierCasualties = a.Casualties.Where(c => c.IsAggressor).Sum(c => c.Soldiers),
                    DefenderMercenaryCasualties = a.Casualties.Where(c => c.IsAggressor).Sum(c => c.Mercenaries),
                    AttackerSoldierCasualties = a.Casualties.Where(c => !c.IsAggressor).Sum(c => c.Soldiers),
                    AttackerMercenaryCasualties = a.Casualties.Where(c => !c.IsAggressor).Sum(c => c.Mercenaries),
                    Result = _formatter.ToString(a.Result),
                    IsRead = a.IsRead
                })
                .ToList();
        }
    }
}