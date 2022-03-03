using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;

namespace WarOfEmpires.QueryHandlers.Alliances {
    public sealed class GetCreateNonAggressionPactRequestQueryHandler : IQueryHandler<GetCreateNonAggressionPactRequestQuery, CreateNonAggressionPactRequestModel> {
        private readonly IReadOnlyWarContext _context;

        public GetCreateNonAggressionPactRequestQueryHandler(IReadOnlyWarContext context) {
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
