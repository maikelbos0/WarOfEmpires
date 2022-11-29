using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Security;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Security {
    public sealed class GetUserClaimsQueryHandler : IQueryHandler<GetUserClaimsQuery, UserClaimsViewModel> {
        private readonly IReadOnlyWarContext _context;

        public GetUserClaimsQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        public UserClaimsViewModel Execute(GetUserClaimsQuery query) {
            var user = _context.Users
                .Include(u => u.RefreshTokenFamilies)
                .Single(u => EmailComparisonService.Equals(u.Email, query.Email));

            var player = _context.Players
                .Include(p => p.AllianceRole)
                .Include(p => p.Alliance.Leader)
                .SingleOrDefault(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            var result = new UserClaimsViewModel() {
                Subject = user.Email,                
                IsAdmin = user.IsAdmin,
                IsPlayer = player != null,
                IsInAlliance = player?.Alliance != null,
                DisplayName = player?.DisplayName
            };
            
            var refreshToken = user.RefreshTokenFamilies.SingleOrDefault(f => f.RequestId == query.RequestId)?.CurrentToken;

            if (refreshToken != null) {
                result.RefreshToken = Convert.ToBase64String(user.RefreshTokenFamilies.Single(f => f.RequestId == query.RequestId).CurrentToken);
            }

            if (player?.Alliance != null) {
                result.IsInAlliance = true;

                if (player == player.Alliance.Leader) {
                    result.CanInvite = true;
                    result.CanManageRoles = true;
                    result.CanTransferLeadership = true;
                    result.CanDisbandAlliance = true;
                    result.CanManageNonAggressionPacts = true;
                    result.CanManageWars = true;
                    result.CanBank = true;
                }
                else {
                    result.CanLeaveAlliance = true;

                    if (player.AllianceRole != null) {
                        result.CanInvite = player.AllianceRole.CanInvite;
                        result.CanManageRoles = player.AllianceRole.CanManageRoles;
                        result.CanManageNonAggressionPacts = player.AllianceRole.CanManageNonAggressionPacts;
                        result.CanManageWars = player.AllianceRole.CanManageWars;
                        result.CanBank = player.AllianceRole.CanBank;
                    }
                }
            }

            return result;
        }
    }
}
