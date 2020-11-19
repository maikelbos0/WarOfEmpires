using System.Data.Entity;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetAllianceDetailsQueryHandler : IQueryHandler<GetAllianceDetailsQuery, AllianceDetailsViewModel> {
        private readonly IWarContext _context;
        private readonly EnumFormatter _formatter;

        public GetAllianceDetailsQueryHandler(IWarContext context, EnumFormatter formatter) {
            _formatter = formatter;
            _context = context;
        }

        public AllianceDetailsViewModel Execute(GetAllianceDetailsQuery query) {
            var alliance = _context.Alliances
                .Include(a => a.Leader)
                .Single(a => a.Id == query.Id);
            var members = _context.Players
                .Include(p => p.Workers)
                .Include(p => p.Troops)
                .Where(p => p.User.Status == UserStatus.Active && p.Alliance.Id == query.Id)
                .OrderBy(p => p.Rank)
                .ToList();

            return new AllianceDetailsViewModel() {
                Id = query.Id,
                Code = alliance.Code,
                Name = alliance.Name,
                LeaderId = alliance.Leader.Id,
                Leader = alliance.Leader.DisplayName,
                Members = members.Select(p => new AllianceMemberViewModel() {
                    Id = p.Id,
                    Rank = p.Rank,
                    DisplayName = p.DisplayName,
                    Title = _formatter.ToString(p.Title),
                    Population = p.Peasants + p.Workers.Sum(w => w.Count) + p.Troops.Sum(t => t.GetTotals())
                }).ToList()
            };
        }
    }
}