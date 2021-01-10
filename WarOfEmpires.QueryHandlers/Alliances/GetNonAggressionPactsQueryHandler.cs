using System.Collections.Generic;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetNonAggressionPactsQueryHandler : IQueryHandler<GetNonAggressionPactsQuery, IEnumerable<NonAggressionPactViewModel>> {
        private readonly IWarContext _context;

        public GetNonAggressionPactsQueryHandler(IWarContext context) {
            _context = context;
        }

        public IEnumerable<NonAggressionPactViewModel> Execute(GetNonAggressionPactsQuery query) {
            throw new System.NotImplementedException();
        }
    }
}
