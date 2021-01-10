using System.Collections.Generic;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetReceivedNonAggressionPactRequestsQueryHandler : IQueryHandler<GetReceivedNonAggressionPactRequestsQuery, IEnumerable<ReceivedNonAggressionPactRequestViewModel>> {
        private readonly IWarContext _context;

        public GetReceivedNonAggressionPactRequestsQueryHandler(IWarContext context) {
            _context = context;
        }

        public IEnumerable<ReceivedNonAggressionPactRequestViewModel> Execute(GetReceivedNonAggressionPactRequestsQuery query) {
            throw new System.NotImplementedException();
        }
    }
}
