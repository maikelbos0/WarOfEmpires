using WarOfEmpires.Database;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetBuildingQueryHandler : IQueryHandler<GetBuildingQuery, BuildingViewModel> {
        private readonly IWarContext _context;

        public GetBuildingQueryHandler(IWarContext context) {
            _context = context;
        }

        public BuildingViewModel Execute(GetBuildingQuery query) {
            throw new System.NotImplementedException();
        }
    }
}