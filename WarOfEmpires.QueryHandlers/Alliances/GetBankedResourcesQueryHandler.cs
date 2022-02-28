using Microsoft.EntityFrameworkCore;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.Utilities.Auditing;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    public class GetBankedResourcesQueryHandler : IQueryHandler<GetBankedResourcesQuery, BankedResourcesModel> {
        private readonly IReadOnlyWarContext _context;
        private readonly IResourcesMap _resourcesMap;

        public GetBankedResourcesQueryHandler(IReadOnlyWarContext context, IResourcesMap resourcesMap) {
            _context = context;
            _resourcesMap = resourcesMap;
        }

        [Audit]
        public BankedResourcesModel Execute(GetBankedResourcesQuery query) {
            var alliance = _context.Players
                .Include(p => p.Alliance)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .Alliance;

            return new BankedResourcesModel() {
                BankedResources = _resourcesMap.ToViewModel(alliance.BankedResources),
                BankTurns = alliance.BankTurns
            };
        }
    }
}
