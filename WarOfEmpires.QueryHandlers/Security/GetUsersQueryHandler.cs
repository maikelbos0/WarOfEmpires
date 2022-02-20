using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Security;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Security {
    public sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, IEnumerable<UserViewModel>> {
        private readonly IReadOnlyWarContext _context;
        private readonly IEnumFormatter _formatter;

        public GetUsersQueryHandler(IReadOnlyWarContext context, IEnumFormatter formatter) {
            _context = context;
            _formatter = formatter;
        }

        [Audit]
        public IEnumerable<UserViewModel> Execute(GetUsersQuery query) {
            var players = _context.Players.AsQueryable();

            if (!string.IsNullOrEmpty(query.DisplayName)) {
                players = players.Where(p => p.DisplayName.Contains(query.DisplayName));
            }

            // Materialize before setting the status
            return players
                .Select(p => new {
                    p.Id,
                    p.User.Email,
                    p.DisplayName,
                    p.Alliance,
                    p.User.Status,
                    p.User.IsAdmin,
                    p.User.LastOnline
                })
                .ToList()
                .Select(u => new UserViewModel() {
                    Id = u.Id,
                    Email = u.Email,
                    DisplayName = u.DisplayName,
                    AllianceCode = u.Alliance?.Code,
                    AllianceName = u.Alliance?.Name,
                    Status = _formatter.ToString(u.Status),
                    IsAdmin = u.IsAdmin,
                    LastOnline = u.LastOnline
                });
        }
    }
}
