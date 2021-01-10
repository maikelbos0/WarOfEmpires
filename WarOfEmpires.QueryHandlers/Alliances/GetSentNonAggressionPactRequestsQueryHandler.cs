using System.Collections.Generic;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Alliances {    
    [InterfaceInjectable]
    [Audit]
    public sealed class GetSentNonAggressionPactRequestsQueryHandler : IQueryHandler<GetSentNonAggressionPactRequestsQuery, IEnumerable<SentNonAggressionPactRequestViewModel>> {
        private readonly IWarContext _context;

        public GetSentNonAggressionPactRequestsQueryHandler(IWarContext context) {
            _context = context;
        }

        public IEnumerable<SentNonAggressionPactRequestViewModel> Execute(GetSentNonAggressionPactRequestsQuery query) {
            throw new System.NotImplementedException();
        }
    }
}
