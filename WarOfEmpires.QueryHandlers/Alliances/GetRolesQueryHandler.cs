using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    public sealed class GetRolesQueryHandler : IQueryHandler<GetRolesQuery, IEnumerable<RoleViewModel>> {
        private readonly IReadOnlyWarContext _context;

        public GetRolesQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        [Audit]
        public IEnumerable<RoleViewModel> Execute(GetRolesQuery query) {
            return _context.Players
                .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.Alliance.Roles)
                .Select(r => new RoleViewModel() {
                    Id = r.Id,
                    Name = r.Name,
                    CanInvite = r.CanInvite,
                    CanManageRoles = r.CanManageRoles,
                    CanDeleteChatMessages = r.CanDeleteChatMessages,
                    CanKickMembers = r.CanKickMembers,
                    CanManageNonAggressionPacts = r.CanManageNonAggressionPacts,
                    CanManageWars = r.CanManageWars,
                    CanBank = r.CanBank,
                    Players = r.Players.Count(p => p.User.Status == UserStatus.Active)
                });
        }
    }
}