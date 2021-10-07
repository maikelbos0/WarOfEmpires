using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Security {
    [TransientServiceImplementation(typeof(IQueryHandler<GetUserIsAdminQuery, bool>))]
    public sealed class GetUserIsAdminQueryHandler : IQueryHandler<GetUserIsAdminQuery, bool> {
        private readonly IReadOnlyWarContext _context;

        public GetUserIsAdminQueryHandler(IReadOnlyWarContext context) {
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