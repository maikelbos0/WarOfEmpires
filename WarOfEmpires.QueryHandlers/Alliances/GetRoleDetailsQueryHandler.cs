using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetRoleDetailsQueryHandler : IQueryHandler<GetRoleDetailsQuery, RoleDetailsViewModel> {
        private readonly IWarContext _context;
        private readonly EnumFormatter _formatter;

        public GetRoleDetailsQueryHandler(IWarContext context, EnumFormatter formatter) {
            _formatter = formatter;
            _context = context;
        }

        public RoleDetailsViewModel Execute(GetRoleDetailsQuery query) {
            throw new System.NotImplementedException();
        }
    }
}