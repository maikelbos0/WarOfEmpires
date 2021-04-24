using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Attacks {
    [ScopedServiceImplementation(typeof(IQueryHandler<GetLastExecutedAttackQuery, int>))]
    public sealed class GetLastExecutedAttackQueryHandler : IQueryHandler<GetLastExecutedAttackQuery, int> {
        private readonly IWarContext _context;

        public GetLastExecutedAttackQueryHandler(IWarContext context) {
            _context = context;
        }

        [Audit]
        public int Execute(GetLastExecutedAttackQuery query) {
            return _context.Players
                .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.ExecutedAttacks)
                .Max(a => a.Id);
        }
    }
}