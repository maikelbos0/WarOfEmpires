using Microsoft.EntityFrameworkCore;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Security;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.QueryHandlers.Decorators;

namespace WarOfEmpires.QueryHandlers.Security {
    public sealed class GetUserDetailsQueryHandler : IQueryHandler<GetUserDetailsQuery, UserDetailsModel> {
        private readonly IReadOnlyWarContext _context;

        public GetUserDetailsQueryHandler(IReadOnlyWarContext context) {
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
