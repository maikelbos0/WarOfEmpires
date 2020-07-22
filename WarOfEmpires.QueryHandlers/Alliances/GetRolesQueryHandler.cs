using System.Collections.Generic;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;

namespace WarOfEmpires.QueryHandlers.Alliances {
    public sealed class GetRolesQueryHandler : IQueryHandler<GetRolesQuery, IEnumerable<RoleViewModel>> {
        public IEnumerable<RoleViewModel> Execute(GetRolesQuery query) {
            throw new System.NotImplementedException();
        }
    }
}