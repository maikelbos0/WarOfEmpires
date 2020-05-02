using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetAlliancesQueryHandler : IQueryHandler<GetAlliancesQuery, IEnumerable<AllianceViewModel>> {
        private readonly IWarContext _context;

        public GetAlliancesQueryHandler(IWarContext context) {
            _context = context;
        }

        public IEnumerable<AllianceViewModel> Execute(GetAlliancesQuery query) {
            var alliances = _context.Alliances
                .Where(a => a.IsActive);

            if (!string.IsNullOrEmpty(query.Name)) {
                alliances = alliances.Where(a => a.Name.Contains(query.Name));
            }

            if (!string.IsNullOrEmpty(query.Code)) {
                alliances = alliances.Where(a => a.Code.Contains(query.Code));
            }

            return alliances.
                Select(a => new AllianceViewModel() {
                    Id = a.Id,
                    Code = a.Code,
                    Name = a.Name,
                    Members = a.Members.Count(),
                    Leader = a.Leader.DisplayName
                })
                .ToList();
        }
    }
}
