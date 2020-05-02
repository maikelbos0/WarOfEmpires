using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Players {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetPlayerIsInAllianceQueryHandler : IQueryHandler<GetPlayerIsInAllianceQuery, bool> {
        private readonly IWarContext _context;

        public GetPlayerIsInAllianceQueryHandler(IWarContext context) {
            _context = context;
        }

        public bool Execute(GetPlayerIsInAllianceQuery query) {
            return _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .Alliance != null;
        }
    }
}