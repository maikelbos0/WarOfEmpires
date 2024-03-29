﻿using Microsoft.EntityFrameworkCore;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    public sealed class GetNewLeaderQueryHandler : IQueryHandler<GetNewLeaderQuery, NewLeadersModel> {
        private readonly IReadOnlyWarContext _context;

        public GetNewLeaderQueryHandler(IReadOnlyWarContext context) {
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
