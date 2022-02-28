using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.Utilities.Auditing;

namespace WarOfEmpires.QueryHandlers.Alliances {
    public sealed class GetInvitePlayerQueryHandler : IQueryHandler<GetInvitePlayerQuery, SendInviteModel> {
        private readonly IReadOnlyWarContext _context;

        public GetInvitePlayerQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        [Audit]
        public SendInviteModel Execute(GetInvitePlayerQuery query) {
            var player = _context.Players
                .Single(p => p.User.Status == UserStatus.Active && p.Id == query.PlayerId);

            return new SendInviteModel() {
                PlayerId = query.PlayerId,
                PlayerName = player.DisplayName
            };
        }
    }
}
