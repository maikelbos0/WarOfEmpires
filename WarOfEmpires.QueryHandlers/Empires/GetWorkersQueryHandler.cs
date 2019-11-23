using System.Data.Entity;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetWorkersQueryHandler : IQueryHandler<GetWorkersQuery, WorkerModel> {
        private readonly IWarContext _context;

        public GetWorkersQueryHandler(IWarContext context) {
            _context = context;
        }

        public WorkerModel Execute(GetWorkersQuery query) {
            var player = _context.Players
                .Include(p => p.User)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            return new WorkerModel() {
                CurrentPeasants = player.Peasants,
                CurrentFarmers = player.Farmers,
                CurrentWoodWorkers = player.WoodWorkers,
                CurrentStoneMasons = player.StoneMasons,
                CurrentOreMiners = player.OreMiners
            };
        }
    }
}