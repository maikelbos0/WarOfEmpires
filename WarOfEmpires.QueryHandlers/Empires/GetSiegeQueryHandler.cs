using WarOfEmpires.Database;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public class GetSiegeQueryHandler : IQueryHandler<GetSiegeQuery, SiegeModel> {
        private readonly IWarContext _context;
        private readonly ResourcesMap _resourcesMap;

        public GetSiegeQueryHandler(IWarContext context, ResourcesMap resourcesMap) {
            _context = context;
            _resourcesMap = resourcesMap;
        }

        public SiegeModel Execute(GetSiegeQuery query) {
            throw new System.NotImplementedException();
        }
    }
}