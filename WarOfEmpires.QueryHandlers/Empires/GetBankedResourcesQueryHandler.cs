using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Empires {
    public sealed class GetBankedResourcesQueryHandler : IQueryHandler<GetBankedResourcesQuery, BankedResourcesViewModel> {
        private readonly IReadOnlyWarContext _context;
        private readonly IResourcesMap _resourcesMap;

        public GetBankedResourcesQueryHandler(IReadOnlyWarContext context, IResourcesMap resourcesMap) {
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