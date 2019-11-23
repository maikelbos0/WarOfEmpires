using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Security {
    [InterfaceInjectable]
    public sealed class GetUserIsAdminQueryHandler : IQueryHandler<GetUserIsAdminQuery, bool> {
        private readonly IWarContext _context;

        public GetUserIsAdminQueryHandler(IWarContext context) {
            _context = context;
        }

        public bool Execute(GetUserIsAdminQuery query) {
            var user = _context.Users
                .Single(p => EmailComparisonService.Equals(p.Email, query.Email));

            return user.IsAdmin;
        }
    }
}