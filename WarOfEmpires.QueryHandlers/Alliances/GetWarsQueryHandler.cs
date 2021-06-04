using System.Collections.Generic;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [TransientServiceImplementation(typeof(IQueryHandler<GetWarsQuery, IEnumerable<WarViewModel>>))]
    public sealed class GetWarsQueryHandler : IQueryHandler<GetWarsQuery, IEnumerable<WarViewModel>> {
        private readonly IWarContext _context;

        public GetWarsQueryHandler(IWarContext context) {
            _context = context;
        }

        [Audit]
        public IEnumerable<WarViewModel> Execute(GetWarsQuery query) {
            throw new System.NotImplementedException();
        }
    }
}
