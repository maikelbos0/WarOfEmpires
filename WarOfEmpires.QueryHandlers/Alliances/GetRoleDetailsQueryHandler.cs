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
    public sealed class GetRoleDetailsQueryHandler : IQueryHandler<GetRoleDetailsQuery, RoleDetailsViewModel> {
        private readonly IWarContext _context;

        public GetRoleDetailsQueryHandler(IWarContext context) {
            _context = context;
        }

        public RoleDetailsViewModel Execute(GetRoleDetailsQuery query) {
            var role = _context.Players
                .Include(p => p.Alliance.Roles)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .Alliance
                .Roles
                .Single(r => r.Id == int.Parse(query.RoleId));

            return new RoleDetailsViewModel() {
                Id = role.Id,
                Name = role.Name,
                CanInvite = role.CanInvite,
                Players = role.Players.Where(p => p.User.Status == UserStatus.Active).Select(p => new RolePlayerViewModel() {
                    Id = p.Id,
                    Rank = p.Rank,
                    DisplayName = p.DisplayName
                }).ToList()
            };
        }
    }
}