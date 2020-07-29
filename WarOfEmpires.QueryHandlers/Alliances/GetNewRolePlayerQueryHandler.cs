using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetNewRolePlayerQueryHandler : IQueryHandler<GetNewRolePlayerQuery, NewRolePlayerModel> {
        public NewRolePlayerModel Execute(GetNewRolePlayerQuery query) {
            throw new System.NotImplementedException();
        }
    }
}