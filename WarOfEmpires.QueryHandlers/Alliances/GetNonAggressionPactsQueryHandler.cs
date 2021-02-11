using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetNonAggressionPactsQueryHandler : IQueryHandler<GetNonAggressionPactsQuery, IEnumerable<NonAggressionPactViewModel>> {
        private readonly IWarContext _context;

        public GetNonAggressionPactsQueryHandler(IWarContext context) {
            _context = context;
        }

        public IEnumerable<NonAggressionPactViewModel> Execute(GetNonAggressionPactsQuery query) {
            var alliance = _context.Players
                .Include(p => p.Alliance.NonAggressionPacts.Select(n => n.Alliances))
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .Alliance;

            return alliance
                .NonAggressionPacts
                .OrderBy(p => p.Id)
                .SelectMany(p => p.Alliances.Select(a => new NonAggressionPactViewModel() {
                    Id = p.Id,
                    AllianceId = a.Id,
                    Code = a.Code,
                    Name = a.Name
                }))
                .Where(p => p.AllianceId != alliance.Id);
        }
    }
}
