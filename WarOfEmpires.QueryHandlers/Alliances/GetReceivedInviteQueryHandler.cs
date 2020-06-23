using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetReceivedInviteQueryHandler : IQueryHandler<GetReceivedInviteQuery, ReceivedInviteDetailsViewModel> {
        private readonly IWarContext _context;

        public GetReceivedInviteQueryHandler(IWarContext context) {
            _context = context;
        }

        public ReceivedInviteDetailsViewModel Execute(GetReceivedInviteQuery query) {
            throw new System.NotImplementedException();
        }
    }
}