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
    public sealed class GetBankedResourcesQueryHandler : IQueryHandler<GetBankedResourcesQuery, BankedResourcesViewModel> {
        private readonly IWarContext _context;
        private readonly ResourcesMap _resourcesMap;

        public GetBankedResourcesQueryHandler(IWarContext context, ResourcesMap resourcesMap) {
            _context = context;
            _resourcesMap = resourcesMap;
        }

        public BankedResourcesViewModel Execute(GetBankedResourcesQuery query) {
            var player = _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            return new BankedResourcesViewModel() {
                BankedResources = _resourcesMap.ToViewModel(player.BankedResources),
                Capacity = _resourcesMap.ToViewModel(player.GetBankCapacity()),
                AvailableCapacity = _resourcesMap.ToViewModel(player.GetAvailableBankCapacity()),
                BankableResources = _resourcesMap.ToViewModel(player.GetBankableResources())
            };
        }
    }
}