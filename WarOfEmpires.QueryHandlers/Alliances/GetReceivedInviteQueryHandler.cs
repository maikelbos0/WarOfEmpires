using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetReceivedInviteQueryHandler : IQueryHandler<GetReceivedInviteQuery, ReceivedInviteDetailsViewModel> {
        private readonly IWarContext _context;

        public GetReceivedInviteQueryHandler(IWarContext context) {
            _context = context;
        }

        public ReceivedInviteDetailsViewModel Execute(GetReceivedInviteQuery query) {
            return _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .Invites
                .OrderBy(i => i.Date)
                .Select(i => new ReceivedInviteDetailsViewModel() {
                    Id = i.Id,
                    Date = i.Date,
                    AllianceId = i.Alliance.Id,
                    AllianceCode = i.Alliance.Code,
                    AllianceName = i.Alliance.Name,
                    Subject = i.Subject,
                    Body = i.Body
                })
                .Single(i => i.Id == int.Parse(query.InviteId));
        }
    }
}