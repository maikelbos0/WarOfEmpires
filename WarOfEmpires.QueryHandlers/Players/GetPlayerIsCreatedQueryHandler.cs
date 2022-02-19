using System.Linq;
using VDT.Core.DependencyInjection.Attributes;
using WarOfEmpires.Database;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Players {
    [TransientServiceImplementation(typeof(IQueryHandler<GetPlayerIsCreatedQuery, bool>))]
    public sealed class GetPlayerIsCreatedQueryHandler : IQueryHandler<GetPlayerIsCreatedQuery, bool> {
        private readonly IReadOnlyWarContext _context;

        public GetPlayerIsCreatedQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        public bool Execute(GetPlayerIsCreatedQuery query) {
            return _context.Players
                .Any(p => EmailComparisonService.Equals(p.User.Email, query.Email));
        }
    }
}
