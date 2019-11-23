using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Security {
    [InterfaceInjectable]
    public sealed class GetUserNewEmailQueryHandler : IQueryHandler<GetUserNewEmailQuery, string> {
        private readonly IWarContext _context;

        public GetUserNewEmailQueryHandler(IWarContext context) {
            _context = context;
        }

        public string Execute(GetUserNewEmailQuery query) {
            var user = _context.Users
                .Single(u => EmailComparisonService.Equals(u.Email, query.Email));

            return user.NewEmail;
        }
    }
}