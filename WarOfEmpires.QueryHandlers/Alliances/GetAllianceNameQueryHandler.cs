using Microsoft.EntityFrameworkCore;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [ScopedServiceImplementation(typeof(IQueryHandler<GetAllianceNameQuery, string>))]
    public sealed class GetAllianceNameQueryHandler : IQueryHandler<GetAllianceNameQuery, string> {
        private readonly IWarContext _context;

        public GetAllianceNameQueryHandler(IWarContext context) {
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