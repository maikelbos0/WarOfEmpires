using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Decorators;

namespace WarOfEmpires.QueryHandlers.Players {
    [TransientServiceImplementation(typeof(IQueryHandler<GetPlayerHomeQuery, PlayerHomeViewModel>))]
    public sealed class GetPlayerHomeQueryHandler : IQueryHandler<GetPlayerHomeQuery, PlayerHomeViewModel> {
        private readonly IWarContext _context;

        public GetPlayerHomeQueryHandler(IWarContext context) {
            _context = context;
        }

        [Audit]
        public PlayerHomeViewModel Execute(GetPlayerHomeQuery query) {
            throw new System.NotImplementedException();
        }
    }
}
