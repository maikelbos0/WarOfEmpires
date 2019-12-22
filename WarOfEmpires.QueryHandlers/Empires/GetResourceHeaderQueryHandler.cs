using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetResourceHeaderQueryHandler : IQueryHandler<GetResourceHeaderQuery, ResourceHeaderViewModel> {
        private readonly IWarContext _context;
        private readonly ResourcesMap _resourcesMap;

        public GetResourceHeaderQueryHandler(IWarContext context, ResourcesMap resourcesMap) {
            _context = context;
            _resourcesMap = resourcesMap;
        }

        public ResourceHeaderViewModel Execute(GetResourceHeaderQuery query) {
            var player = _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            return new ResourceHeaderViewModel() {
                Resources = _resourcesMap.ToViewModel(player.Resources),
                AttackTurns = player.AttackTurns
            };
        }
    }
}