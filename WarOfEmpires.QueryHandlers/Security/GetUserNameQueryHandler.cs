using System.Data.Entity;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

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
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            return player.DisplayName;
        }
    }
}