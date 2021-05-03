using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Empires {
    [ScopedServiceImplementation(typeof(IQueryHandler<GetBankedResourcesQuery, BankedResourcesViewModel>))]
    public sealed class GetBankedResourcesQueryHandler : IQueryHandler<GetBankedResourcesQuery, BankedResourcesViewModel> {
        private readonly IWarContext _context;
        private readonly IResourcesMap _resourcesMap;

        public GetBankedResourcesQueryHandler(IWarContext context, IResourcesMap resourcesMap) {
            _context = context;
            _resourcesMap = resourcesMap;
        }

        [Audit]
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