using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetCreateNonAggressionPactRequestQuery : IQuery<CreateNonAggressionPactRequestModel> {
        public int AllianceId { get; }

        public GetCreateNonAggressionPactRequestQuery(int allianceId) {
            AllianceId = allianceId;
        }
    }
}
