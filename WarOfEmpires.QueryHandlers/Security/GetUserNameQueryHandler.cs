using WarOfEmpires.Database;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.Utilities.Container;
using System;
using System.Linq;
using System.Data.Entity;

namespace WarOfEmpires.QueryHandlers.Security {
    [InterfaceInjectable]
    public sealed class GetUserNameQueryHandler : IQueryHandler<GetUserNameQuery, string> {
        private readonly IWarContext _context;

        public GetUserNameQueryHandler(IWarContext context) {
            _context = context;
        }

        public string Execute(GetUserNameQuery query) {
            var player = _context.Players
                .Include(p => p.User)
                .Single(p => p.User.Email.Equals(query.Email, StringComparison.InvariantCultureIgnoreCase));

            return player.DisplayName;
        }
    }
}