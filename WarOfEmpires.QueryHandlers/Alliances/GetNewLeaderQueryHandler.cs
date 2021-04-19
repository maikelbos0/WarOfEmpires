using Microsoft.EntityFrameworkCore;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [ScopedServiceImplementation(typeof(IQueryHandler<GetNewLeaderQuery, NewLeadersModel>))]
    [Audit]
    public sealed class GetNewLeaderQueryHandler : IQueryHandler<GetNewLeaderQuery, NewLeadersModel> {
        private readonly IWarContext _context;

        public GetNewLeaderQueryHandler(IWarContext context) {
            _context = context;
        }

        public NewLeadersModel Execute(GetNewLeaderQuery query) {
            var alliance = _context.Players
                .Include(p => p.Alliance.Members)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .Alliance;


            return new NewLeadersModel() {
                Members = alliance.Members
                    .Where(p => alliance.Leader != p && p.User.Status == UserStatus.Active)
                    .OrderBy(p => p.DisplayName)
                    .Select(p => new NewLeaderModel() {
                        Id = p.Id,
                        DisplayName = p.DisplayName
                    })
                    .ToList()
            };
        }
    }
}
