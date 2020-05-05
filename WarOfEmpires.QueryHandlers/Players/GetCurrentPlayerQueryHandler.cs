using WarOfEmpires.Database;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Players {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetCurrentPlayerQueryHandler : IQueryHandler<GetCurrentPlayerQuery, CurrentPlayerViewModel> {
        private readonly IWarContext _context;

        public GetCurrentPlayerQueryHandler(IWarContext context) {
            _context = context;
        }

        public CurrentPlayerViewModel Execute(GetCurrentPlayerQuery query) {
            throw new System.NotImplementedException();
        }
    }
}