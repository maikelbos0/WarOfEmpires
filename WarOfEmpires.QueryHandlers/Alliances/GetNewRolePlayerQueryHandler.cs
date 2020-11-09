using System.Data.Entity;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetNewRolePlayerQueryHandler : IQueryHandler<GetNewRolePlayerQuery, NewRolePlayersModel> {
        private readonly IWarContext _context;

        public GetNewRolePlayerQueryHandler(IWarContext context) {
            _context = context;
        }

        public NewRolePlayersModel Execute(GetNewRolePlayerQuery query) {
            var alliance = _context.Players
                .Include(p => p.Alliance.Roles)
                .Include(p => p.Alliance.Members)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .Alliance;

            var role = alliance.Roles.Single(r => r.Id == query.RoleId);

            return new NewRolePlayersModel() {
                Id = role.Id,
                Name = role.Name,
                Players = alliance.Members
                    .Where(p => p.AllianceRole != role && p.User.Status == UserStatus.Active)
                    .OrderBy(p => p.DisplayName)
                    .Select(p => new NewRolePlayerModel() {
                        Id = p.Id,
                        DisplayName = p.DisplayName
                    })
                    .ToList()
            };
        }
    }
}