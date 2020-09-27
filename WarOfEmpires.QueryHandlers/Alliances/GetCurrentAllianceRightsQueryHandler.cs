﻿using System.Data.Entity;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetCurrentAllianceRightsQueryHandler : IQueryHandler<GetCurrentAllianceRightsQuery, CurrentAllianceRightsViewModel> {
        private readonly IWarContext _context;

        public GetCurrentAllianceRightsQueryHandler(IWarContext context) {
            _context = context;
        }

        public CurrentAllianceRightsViewModel Execute(GetCurrentAllianceRightsQuery query) {
            var player = _context.Players
                .Include(p => p.Alliance.Leader)
                .Include(p => p.AllianceRole)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            var result = new CurrentAllianceRightsViewModel() {
                IsInAlliance = player.Alliance != null
            };

            if (player == player.Alliance?.Leader) {
                result.CanInvite = true;
            }
            else if (player.AllianceRole != null) {
                result.CanInvite = player.AllianceRole.CanInvite;
            }

            return result;
        }
    }
}