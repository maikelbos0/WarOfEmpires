using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetCurrentAllianceRightsQueryHandler : IQueryHandler<GetCurrentAllianceRightsQuery, CurrentAllianceRightsViewModel> {
        private readonly IWarContext _context;

        public GetCurrentAllianceRightsQueryHandler(IWarContext context) {
            _context = context;
        }

        public CurrentAllianceRightsViewModel Execute(GetCurrentAllianceRightsQuery query) {
            throw new System.NotImplementedException();
        }
    }
}