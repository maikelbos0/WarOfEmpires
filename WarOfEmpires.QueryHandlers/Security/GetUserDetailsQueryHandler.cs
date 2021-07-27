﻿using Microsoft.EntityFrameworkCore;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Security;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.QueryHandlers.Decorators;

namespace WarOfEmpires.QueryHandlers.Security {
    [TransientServiceImplementation(typeof(IQueryHandler<GetUserDetailsQuery, UserDetailsModel>))]
    public sealed class GetUserDetailsQueryHandler : IQueryHandler<GetUserDetailsQuery, UserDetailsModel> {
        private readonly IWarContext _context;

        public GetUserDetailsQueryHandler(IWarContext context) {
            _context = context;
        }

        [Audit]
        public UserDetailsModel Execute(GetUserDetailsQuery query) {            
            var player = _context.Players
                .Include(p => p.User)
                .Include(p => p.Alliance)
                .Where(p => p.Id == query.Id)
                .Single();

            return new UserDetailsModel() {
                Id = player.Id,
                Email = player.User.Email,
                DisplayName = player.DisplayName,
                AllianceCode = player.Alliance?.Code,
                AllianceName = player.Alliance?.Name,
                Status = player.User.Status.ToString(),
                IsAdmin = player.User.IsAdmin,
                LastOnline = player.User.LastOnline
            };
        }
    }
}