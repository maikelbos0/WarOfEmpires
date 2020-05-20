using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetInvitePlayerQueryHandler : IQueryHandler<GetInvitePlayerQuery, SendInviteModel> {
        private readonly IWarContext _context;

        public GetInvitePlayerQueryHandler(IWarContext context) {
            _context = context;
        }

        public SendInviteModel Execute(GetInvitePlayerQuery query) {
            var playerId = int.Parse(query.PlayerId);
            var player = _context.Players
                .Single(p => p.User.Status == UserStatus.Active && p.Id == playerId);

            return new SendInviteModel() {
                PlayerId = query.PlayerId,
                PlayerName = player.DisplayName
            };
        }
    }
}
