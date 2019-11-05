using WarOfEmpires.Database;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.Utilities.Container;
using System;
using System.Linq;

namespace WarOfEmpires.QueryHandlers.Security {
    [InterfaceInjectable]
    public sealed class GetUserNewEmailQueryHandler : IQueryHandler<GetUserNewEmailQuery, string> {
        private readonly IWarContext _context;

        public GetUserNewEmailQueryHandler(IWarContext context) {
            _context = context;
        }

        public string Execute(GetUserNewEmailQuery query) {
            var user = _context.Users
                .Single(u => u.Email.Equals(query.Email, StringComparison.InvariantCultureIgnoreCase));

            return user.NewEmail;
        }
    }
}