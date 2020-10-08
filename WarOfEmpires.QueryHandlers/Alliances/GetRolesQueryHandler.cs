using System.Collections.Generic;
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
    public sealed class GetRolesQueryHandler : IQueryHandler<GetRolesQuery, IEnumerable<RoleViewModel>> {
        private readonly IWarContext _context;

        public GetRolesQueryHandler(IWarContext context) {
            _context = context;
        }

        public IEnumerable<RoleViewModel> Execute(GetRolesQuery query) {
            var alliance = _context.Players
                .Include(p => p.Alliance.Roles)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .Alliance;

            return alliance.Roles.Select(r => new RoleViewModel() {
                Id = r.Id,
                Name = r.Name,
                CanInvite = r.CanInvite,
                CanManageRoles = r.CanManageRoles,
                CanDeleteChatMessages = r.CanDeleteChatMessages,
                Players = r.Players.Count(p => p.User.Status == UserStatus.Active)
            });
        }
    }
}