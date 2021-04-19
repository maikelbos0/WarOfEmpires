using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [ScopedServiceImplementation(typeof(IQueryHandler<GetInvitePlayerQuery, SendInviteModel>))]
    [Audit]
    public sealed class GetInvitePlayerQueryHandler : IQueryHandler<GetInvitePlayerQuery, SendInviteModel> {
        private readonly IWarContext _context;

        public GetInvitePlayerQueryHandler(IWarContext context) {
            _context = context;
        }

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
