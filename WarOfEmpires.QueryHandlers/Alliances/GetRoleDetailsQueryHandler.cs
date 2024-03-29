﻿using Microsoft.EntityFrameworkCore;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    public sealed class GetRoleDetailsQueryHandler : IQueryHandler<GetRoleDetailsQuery, RoleDetailsViewModel> {
        private readonly IReadOnlyWarContext _context;

        public GetRoleDetailsQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        public RoleDetailsViewModel Execute(GetRoleDetailsQuery query) {
            var role = _context.Players
                .Include(p => p.Alliance.Roles).ThenInclude(r => r.Players).ThenInclude(rp => rp.User)
                .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.Alliance.Roles)
                .Single(r => r.Id == query.RoleId);

            return new RoleDetailsViewModel() {
                Id = role.Id,
                Name = role.Name,
                CanInvite = role.CanInvite,
                CanManageRoles = role.CanManageRoles,
                CanDeleteChatMessages = role.CanDeleteChatMessages,
                CanKickMembers = role.CanKickMembers,
                CanManageNonAggressionPacts = role.CanManageNonAggressionPacts,
                CanManageWars = role.CanManageWars,
                CanBank = role.CanBank,
                Players = role.Players.Where(p => p.User.Status == UserStatus.Active).Select(p => new RolePlayerViewModel() {
                    Id = p.Id,
                    Rank = p.Rank,
                    DisplayName = p.DisplayName
                }).ToList()
            };
        }
    }
}