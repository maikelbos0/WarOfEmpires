using System.Data.Entity;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Formatting;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetAllianceHomeQueryHandler : IQueryHandler<GetAllianceHomeQuery, AllianceHomeViewModel> {
        private readonly IWarContext _context;
        private readonly EnumFormatter _formatter;

        public GetAllianceHomeQueryHandler(IWarContext context, EnumFormatter formatter) {
            _formatter = formatter;
            _context = context;
        }

        public AllianceHomeViewModel Execute(GetAllianceHomeQuery query) {
            var alliance = _context.Players
                .Include(p => p.Alliance.Leader)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .Alliance;
            var members = _context.Players
                .Include(p => p.Workers)
                .Include(p => p.Troops)
                .Include(p => p.User)
                .Where(p => p.User.Status == UserStatus.Active && p.Alliance.Id == alliance.Id)
                .OrderBy(p => p.Rank)
                .ToList();

            return new AllianceHomeViewModel() {
                Id = alliance.Id,
                Code = alliance.Code,
                Name = alliance.Name,
                LeaderId = alliance.Leader.Id,
                Leader = alliance.Leader.DisplayName,
                Members = members.Select(p => new AllianceHomeMemberViewModel() {
                    Id = p.Id,
                    LastOnline = p.User.LastOnline,
                    Rank = p.Rank,
                    DisplayName = p.DisplayName,
                    Title = _formatter.ToString(p.Title),
                    Population = p.Peasants + p.Workers.Sum(w => w.Count) + p.Troops.Sum(t => t.GetTotals())
                }).ToList()
            };
        }
    }
}