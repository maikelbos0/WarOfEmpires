using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [ScopedServiceImplementation(typeof(IQueryHandler<GetCreateNonAggressionPactRequestQuery, CreateNonAggressionPactRequestModel>))]
    [Audit]
    public sealed class GetCreateNonAggressionPactRequestQueryHandler : IQueryHandler<GetCreateNonAggressionPactRequestQuery, CreateNonAggressionPactRequestModel> {
        private readonly IWarContext _context;

        public GetCreateNonAggressionPactRequestQueryHandler(IWarContext context) {
            _context = context;
        }

        public CreateNonAggressionPactRequestModel Execute(GetCreateNonAggressionPactRequestQuery query) {
            return _context.Alliances.Select(a => new CreateNonAggressionPactRequestModel() {
                AllianceId = a.Id,
                AllianceCode = a.Code,
                AllianceName = a.Name
            }).Single(a => a.AllianceId == query.AllianceId);
        }
    }
}
