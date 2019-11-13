using WarOfEmpires.Database;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.Utilities.Container;
using System;
using System.Linq;

namespace WarOfEmpires.QueryHandlers.Security {
    [InterfaceInjectable]
    public sealed class GetUserIsAdminQueryHandler : IQueryHandler<GetUserIsAdminQuery, bool> {
        private readonly IWarContext _context;

        public GetUserIsAdminQueryHandler(IWarContext context) {
            _context = context;
        }

        public bool Execute(GetUserIsAdminQuery query) {
            var user = _context.Users
                .Single(p => p.Email.Equals(query.Email, StringComparison.InvariantCultureIgnoreCase));

            return user.IsAdmin;
        }
    }
}