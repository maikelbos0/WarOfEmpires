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
            var role = _context.Players
                .Where(p => p.Alliance != null && EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.Alliance.Roles)
                .Single(r => r.Id == query.RoleId);

            var players = _context.Players
                .Where(p => p.Alliance != null && EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.Alliance.Members)
                .Where(p => p.AllianceRole.Id != role.Id && p.User.Status == UserStatus.Active)
                .OrderBy(p => p.DisplayName)
                .Select(p => new NewRolePlayerModel() {
                    Id = p.Id,
                    DisplayName = p.DisplayName
                })
                .ToList();

            return new NewRolePlayersModel() {
                Id = role.Id,
                Name = role.Name,
                Players = players
            };
        }
    }
}