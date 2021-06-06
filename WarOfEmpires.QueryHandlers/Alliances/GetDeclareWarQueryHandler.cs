using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [TransientServiceImplementation(typeof(IQueryHandler<GetDeclareWarQuery, DeclareWarModel>))]
    public sealed class GetDeclareWarQueryHandler : IQueryHandler<GetDeclareWarQuery, DeclareWarModel> {
        private readonly IWarContext _context;

        public GetDeclareWarQueryHandler(IWarContext context) {
            _context = context;
        }

        [Audit]
        public DeclareWarModel Execute(GetDeclareWarQuery query) {
            return _context.Alliances.Select(a => new DeclareWarModel() {
                AllianceId = a.Id,
                AllianceCode = a.Code,
                AllianceName = a.Name
            }).Single(a => a.AllianceId == query.AllianceId);
        }
    }
}
