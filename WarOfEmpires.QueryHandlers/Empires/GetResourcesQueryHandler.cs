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
    public sealed class GetResourcesQueryHandler : IQueryHandler<GetResourcesQuery, ResourcesViewModel> {
        private readonly IWarContext _context;

        public GetResourcesQueryHandler(IWarContext context) {
            _context = context;
        }

        public ResourcesViewModel Execute(GetResourcesQuery query) {
            var player = _context.Players
                .Include(p => p.User)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            return new ResourcesViewModel() {
                Gold = player.Resources.Gold,
                Food = player.Resources.Food,
                Wood = player.Resources.Wood,
                Stone = player.Resources.Stone,
                Ore = player.Resources.Ore
            };
        }
    }
}