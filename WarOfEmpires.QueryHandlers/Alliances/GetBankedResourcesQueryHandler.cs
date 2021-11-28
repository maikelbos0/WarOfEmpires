using Microsoft.EntityFrameworkCore;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [TransientServiceImplementation(typeof(IQueryHandler<GetBankedResourcesQuery, BankedResourcesViewModel>))]
    public class GetBankedResourcesQueryHandler : IQueryHandler<GetBankedResourcesQuery, BankedResourcesViewModel> {
        private readonly IReadOnlyWarContext _context;
        private readonly IResourcesMap _resourcesMap;

        public GetBankedResourcesQueryHandler(IReadOnlyWarContext context, IResourcesMap resourcesMap) {
            _context = context;
            _resourcesMap = resourcesMap;
        }

        [Audit]
        public BankedResourcesViewModel Execute(GetBankedResourcesQuery query) {
            var alliance = _context.Players
                .Include(p => p.Alliance)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .Alliance;

            return new BankedResourcesViewModel() {
                BankedResources = _resourcesMap.ToViewModel(alliance.BankedResources),
                BankTurns = alliance.BankTurns
            };
        }
    }
}
