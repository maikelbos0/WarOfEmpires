using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetInviteQueryHandler : IQueryHandler<GetInviteQuery, InviteDetailsViewModel> {
        private readonly IWarContext _context;

        public GetInviteQueryHandler(IWarContext context) {
            _context = context;
        }

        public InviteDetailsViewModel Execute(GetInviteQuery query) {
            throw new System.NotImplementedException();
        }
    }
}