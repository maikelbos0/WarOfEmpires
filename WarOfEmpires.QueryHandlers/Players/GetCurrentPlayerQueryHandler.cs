﻿using System.Data.Entity;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Players {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetCurrentPlayerQueryHandler : IQueryHandler<GetCurrentPlayerQuery, CurrentPlayerViewModel> {
        private readonly IWarContext _context;

        public GetCurrentPlayerQueryHandler(IWarContext context) {
            _context = context;
        }

        public CurrentPlayerViewModel Execute(GetCurrentPlayerQuery query) {
            var player = _context.Players
                .Include(p => p.User)
                .Include(p => p.Alliance)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            return new CurrentPlayerViewModel() {
                IsAuthenticated = true,
                IsAdmin = player.User.IsAdmin,
                IsInAlliance = player.Alliance != null,
                DisplayName = player.DisplayName
            };                
        }
    }
}