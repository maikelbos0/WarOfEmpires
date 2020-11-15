using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetNewLeaderQueryHandler : IQueryHandler<GetNewLeaderQuery, NewLeadersModel> {
        private readonly IWarContext _context;

        public GetNewLeaderQueryHandler(IWarContext context) {
            _context = context;
        }

        public NewLeadersModel Execute(GetNewLeaderQuery query) {
            throw new System.NotImplementedException();
        }
    }
}
