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
    [ScopedServiceImplementation(typeof(IQueryHandler<GetReceivedAttacksQuery, IEnumerable<ReceivedAttackViewModel>>))]
    [Audit]
    public sealed class GetReceivedAttacksQueryHandler : IQueryHandler<GetReceivedAttacksQuery, IEnumerable<ReceivedAttackViewModel>> {
        private readonly IWarContext _context;
        private readonly EnumFormatter _formatter;

        public GetReceivedAttacksQueryHandler(IWarContext context, EnumFormatter formatter) {
            _context = context;
            _formatter = formatter;
        }

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
                    DefenderSoldierCasualties = (int?)a.Rounds.Where(r => r.IsAggressor).Sum(r => r.Casualties.Sum(c => c.Soldiers)),
                    DefenderMercenaryCasualties = (int?)a.Rounds.Where(r => r.IsAggressor).Sum(r => r.Casualties.Sum(c => c.Mercenaries)),
                    AttackerSoldierCasualties = (int?)a.Rounds.Where(r => !r.IsAggressor).Sum(r => r.Casualties.Sum(c => c.Soldiers)),
                    AttackerMercenaryCasualties = (int?)a.Rounds.Where(r => !r.IsAggressor).Sum(r => r.Casualties.Sum(c => c.Mercenaries)),
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
                    DefenderSoldierCasualties = a.DefenderSoldierCasualties ?? 0,
                    DefenderMercenaryCasualties = a.DefenderMercenaryCasualties ?? 0,
                    AttackerSoldierCasualties = a.AttackerSoldierCasualties ?? 0,
                    AttackerMercenaryCasualties = a.AttackerMercenaryCasualties ?? 0,
                    Result = _formatter.ToString(a.Result),
                    IsRead = a.IsRead
                })
                .ToList();
        }
    }
}