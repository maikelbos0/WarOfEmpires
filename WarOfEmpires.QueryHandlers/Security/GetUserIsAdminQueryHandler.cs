using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Security {
    [ScopedServiceImplementation(typeof(IQueryHandler<GetUserIsAdminQuery, bool>))]
    public sealed class GetUserIsAdminQueryHandler : IQueryHandler<GetUserIsAdminQuery, bool> {
        private readonly IWarContext _context;

        public GetUserIsAdminQueryHandler(IWarContext context) {
            _context = context;
        }

        public bool Execute(GetUserIsAdminQuery query) {
            return _context.Users
                .Where(p => EmailComparisonService.Equals(p.Email, query.Email))
                .Select(p => p.IsAdmin)
                .Single();
        }
    }
}