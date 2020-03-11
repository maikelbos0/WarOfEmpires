using WarOfEmpires.Database;
using WarOfEmpires.Models.Markets;
using WarOfEmpires.Queries.Markets;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Markets {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetCaravansQueryHandler : IQueryHandler<GetCaravansQuery, CaravansModel> {

        private readonly IWarContext _context;

        public GetCaravansQueryHandler(IWarContext context) {
            _context = context;
        }

        public CaravansModel Execute(GetCaravansQuery query) {
            throw new System.NotImplementedException();
        }
    }
}