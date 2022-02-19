using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using VDT.Core.DependencyInjection.Attributes;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [TransientServiceImplementation(typeof(IQueryHandler<GetAlliancesQuery, IEnumerable<AllianceViewModel>>))]
    public sealed class GetAlliancesQueryHandler : IQueryHandler<GetAlliancesQuery, IEnumerable<AllianceViewModel>> {
        private readonly IReadOnlyWarContext _context;

        public GetAlliancesQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        [Audit]
        public IEnumerable<AllianceViewModel> Execute(GetAlliancesQuery query) {
            var currentAllianceId = _context.Players
                .Include(p => p.Alliance)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .Alliance?.Id;
            var alliances = _context.Alliances.AsQueryable();

            if (!string.IsNullOrEmpty(query.Name)) {
                alliances = alliances.Where(a => a.Name.Contains(query.Name));
            }

            if (!string.IsNullOrEmpty(query.Code)) {
                alliances = alliances.Where(a => a.Code.Contains(query.Code));
            }

            return alliances.
                Select(a => new AllianceViewModel() {
                    Id = a.Id,
                    Status = a.Id == currentAllianceId ? "Mine" : a.NonAggressionPacts.Any(p => p.Alliances.Any(pa => pa.Id == currentAllianceId)) ? "Pact" : a.Wars.Any(w => w.Alliances.Any(wa => wa.Id == currentAllianceId)) ? "War" : null,
                    Code = a.Code,
                    Name = a.Name,
                    Members = a.Members.Count,
                    Leader = a.Leader.DisplayName
                })
                .ToList();
        }
    }
}
