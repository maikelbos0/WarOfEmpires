using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Empires {
    public sealed class GetResourceHeaderQueryHandler : IQueryHandler<GetResourceHeaderQuery, ResourceHeaderViewModel> {
        private readonly IReadOnlyWarContext _context;  
        private readonly IResourcesMap _resourcesMap;

        public GetResourceHeaderQueryHandler(IReadOnlyWarContext context, IResourcesMap resourcesMap) {
            _context = context;
            _resourcesMap = resourcesMap;
        }

        [Audit]
        public ResourceHeaderViewModel Execute(GetResourceHeaderQuery query) {
            var player = _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            return new ResourceHeaderViewModel() {
                Resources = _resourcesMap.ToViewModel(player.Resources),
                BankedResources = _resourcesMap.ToViewModel(player.BankedResources),
                AttackTurns = player.AttackTurns,
                BankTurns = player.BankTurns
            };
        }
    }
}