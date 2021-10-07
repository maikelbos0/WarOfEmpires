using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Security {
    [TransientServiceImplementation(typeof(IQueryHandler<GetUserNewEmailQuery, string>))]
    public sealed class GetUserNewEmailQueryHandler : IQueryHandler<GetUserNewEmailQuery, string> {
        private readonly IReadOnlyWarContext _context;

        public GetUserNewEmailQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        public string Execute(GetUserNewEmailQuery query) {
            return _context.Users
                .Where(u => EmailComparisonService.Equals(u.Email, query.Email))
                .Select(u => u.NewEmail)
                .Single();
        }
    }
}