using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.Utilities.Auditing;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    public sealed class GetInvitesQueryHandler : IQueryHandler<GetInvitesQuery, IEnumerable<InviteViewModel>> {
        private readonly IReadOnlyWarContext _context;

        public GetInvitesQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        [Audit]
        public IEnumerable<InviteViewModel> Execute(GetInvitesQuery query) {
            return _context.Players
                .Where(p => p.Alliance != null && EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.Alliance.Invites)
                .Where(i => i.Player.User.Status == UserStatus.Active)
                .Select(i => new InviteViewModel() {
                    Id = i.Id,
                    PlayerId = i.Player.Id,
                    PlayerName = i.Player.DisplayName,
                    IsRead = i.IsRead,
                    Date = i.Date,
                    Subject = i.Subject
                })
                .ToList();
        }
    }
}