﻿using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Attacks {
    public sealed class GetLastExecutedAttackQueryHandler : IQueryHandler<GetLastExecutedAttackQuery, int> {
        private readonly IReadOnlyWarContext _context;

        public GetLastExecutedAttackQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        public int Execute(GetLastExecutedAttackQuery query) {
            return _context.Players
                .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.ExecutedAttacks)
                .Max(a => a.Id);
        }
    }
}