using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Attacks {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetLastExecutedAttackQueryHandler : IQueryHandler<GetLastExecutedAttackQuery, int> {
        private readonly IWarContext _context;

        public GetLastExecutedAttackQueryHandler(IWarContext context) {
            _context = context;
        }

        public int Execute(GetLastExecutedAttackQuery query) {
            return _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .ExecutedAttacks
                .Max(a => a.Id);
        }
    }
}