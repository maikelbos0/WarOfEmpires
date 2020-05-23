using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetInvitesQueryHandler : IQueryHandler<GetInvitesQuery, InvitesViewModel> {
        private readonly IWarContext _context;

        public GetInvitesQueryHandler(IWarContext context) {
            _context = context;
        }

        public InvitesViewModel Execute(GetInvitesQuery query) {
            var alliance = _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .Alliance;

            var invites = alliance
                .Invites
                .Where(i => i.Player.User.Status == UserStatus.Active)
                .OrderBy(i => i.Date)
                .ToList();

            return new InvitesViewModel() {
                Name = alliance.Name,
                Invites = invites.Select(i => new InviteViewModel() {
                    Id = i.Id,
                    PlayerName = i.Player.DisplayName,
                    IsRead = i.IsRead,
                    Date = i.Date,
                    Message = i.Message
                }).ToList()
            };
        }
    }
}