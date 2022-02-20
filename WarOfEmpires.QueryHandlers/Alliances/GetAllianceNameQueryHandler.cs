using Microsoft.EntityFrameworkCore;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    public sealed class GetAllianceNameQueryHandler : IQueryHandler<GetAllianceNameQuery, string> {
        private readonly IReadOnlyWarContext _context;

        public GetAllianceNameQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        [Audit]
        public string Execute(GetAllianceNameQuery query) {
            return _context.Players
                .Include(p => p.Alliance)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .Alliance.Name;
        }
    }
}