using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.Utilities.Auditing;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    public sealed class GetReceivedInviteQueryHandler : IQueryHandler<GetReceivedInviteQuery, ReceivedInviteDetailsViewModel> {
        private readonly IReadOnlyWarContext _context;

        public GetReceivedInviteQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        [Audit]
        public ReceivedInviteDetailsViewModel Execute(GetReceivedInviteQuery query) {
            return _context.Players
                .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.Invites)
                .Select(i => new ReceivedInviteDetailsViewModel() {
                    Id = i.Id,
                    Date = i.Date,
                    IsRead = i.IsRead,
                    AllianceId = i.Alliance.Id,
                    AllianceCode = i.Alliance.Code,
                    AllianceName = i.Alliance.Name,
                    Subject = i.Subject,
                    Body = i.Body
                })
                .Single(i => i.Id == query.InviteId);
        }
    }
}