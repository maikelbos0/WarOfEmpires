using WarOfEmpires.Database;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.Utilities.Container;
using System;
using System.Linq;

namespace WarOfEmpires.QueryHandlers.Security {
    [InterfaceInjectable]
    public sealed class GetUserNameQueryHandler : IQueryHandler<GetUserNameQuery, string> {
        private readonly IWarContext _context;

        public GetUserNameQueryHandler(IWarContext context) {
            _context = context;
        }

        public string Execute(GetUserNameQuery query) {
            var user = _context.Users
                .Single(u => u.Email.Equals(query.Email, StringComparison.InvariantCultureIgnoreCase));

            return user.DisplayName ?? user.Email;
        }
    }
}