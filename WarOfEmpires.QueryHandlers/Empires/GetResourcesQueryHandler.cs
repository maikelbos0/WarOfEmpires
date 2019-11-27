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
        private readonly ResourcesMap _resourcesMap;

        public GetResourcesQueryHandler(IWarContext context, ResourcesMap resourcesMap) {
            _context = context;
            _resourcesMap = resourcesMap;
        }

        public ResourcesViewModel Execute(GetResourcesQuery query) {
            var player = _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            return _resourcesMap.ToViewModel(player.Resources);
        }
    }
}