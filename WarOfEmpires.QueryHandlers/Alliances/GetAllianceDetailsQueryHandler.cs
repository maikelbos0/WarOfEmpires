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
            var id = int.Parse(query.Id);
            var alliance = _context.Alliances
                .Single(a => a.Id == id);
            var members = _context.Players
                .Include(p => p.Workers)
                .Include(p => p.Troops)
                .Where(p => p.User.Status == UserStatus.Active && p.Alliance.Id == id)
                .OrderBy(p => p.Rank)
                .ToList();

            return new AllianceDetailsViewModel() {
                Id = id,
                Code = alliance.Code,
                Name = alliance.Name,
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