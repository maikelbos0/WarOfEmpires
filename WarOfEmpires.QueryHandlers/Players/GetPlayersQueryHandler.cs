using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Players {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetPlayersQueryHandler : IQueryHandler<GetPlayersQuery, List<PlayerViewModel>> {
        private readonly IWarContext _context;

        public GetPlayersQueryHandler(IWarContext context) {
            _context = context;
        }

        public List<PlayerViewModel> Execute(GetPlayersQuery query) {
            return _context.Players
                .Where(p => p.User.Status == UserStatus.Active)
                .Select(p => new PlayerViewModel() {
                    Id = p.Id,
                    DisplayName = p.DisplayName,
                    Population = p.Farmers + p.WoodWorkers + p.StoneMasons + p.OreMiners + p.Peasants
                        + p.Archers.GetTotals() + p.Cavalry.GetTotals() + p.Footmen.GetTotals()
                })
                .ToList();
        }
    }
}