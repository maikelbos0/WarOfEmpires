using System.Collections.Generic;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetRolesQueryHandler : IQueryHandler<GetRolesQuery, IEnumerable<RoleViewModel>> {
        private readonly IWarContext _context;

        public GetRolesQueryHandler(IWarContext context) {
            _context = context;
        }

        public IEnumerable<RoleViewModel> Execute(GetRolesQuery query) {
            throw new System.NotImplementedException();
        }
    }
}