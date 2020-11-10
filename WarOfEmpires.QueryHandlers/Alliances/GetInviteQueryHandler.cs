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
    public sealed class GetInviteQueryHandler : IQueryHandler<GetInviteQuery, InviteDetailsViewModel> {
        private readonly IWarContext _context;

        public GetInviteQueryHandler(IWarContext context) {
            _context = context;
        }

        public InviteDetailsViewModel Execute(GetInviteQuery query) {
            return _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .Alliance
                .Invites
                .Where(i => i.Player.User.Status == UserStatus.Active)
                .Select(i => new InviteDetailsViewModel() {
                    Id = i.Id,
                    PlayerId = i.Player.Id,
                    PlayerName = i.Player.DisplayName,
                    IsRead = i.IsRead,
                    Date = i.Date,
                    Subject = i.Subject,
                    Body = i.Body
                })
                .Single(i => i.Id == query.InviteId);
        }
    }
}