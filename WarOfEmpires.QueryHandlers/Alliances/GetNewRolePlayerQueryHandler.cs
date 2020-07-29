using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetNewRolePlayerQueryHandler : IQueryHandler<GetNewRolePlayerQuery, NewRolePlayersModel> {
        private readonly IWarContext _context;

        public GetNewRolePlayerQueryHandler(IWarContext context) {
            _context = context;
        }

        public NewRolePlayersModel Execute(GetNewRolePlayerQuery query) {
            throw new System.NotImplementedException();
        }
    }
}