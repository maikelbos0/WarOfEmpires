using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;

namespace WarOfEmpires.QueryHandlers.Alliances {
    public sealed class GetCreateNonAggressionPactRequestQueryHandler : IQueryHandler<GetCreateNonAggressionPactRequestQuery, CreateNonAggressionPactRequestModel> {
        private readonly IReadOnlyWarContext _context;

        public GetCreateNonAggressionPactRequestQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        [Audit]
        public CreateNonAggressionPactRequestModel Execute(GetCreateNonAggressionPactRequestQuery query) {
            return _context.Alliances.Select(a => new CreateNonAggressionPactRequestModel() {
                AllianceId = a.Id,
                AllianceCode = a.Code,
                AllianceName = a.Name
            }).Single(a => a.AllianceId == query.AllianceId);
        }
    }
}
