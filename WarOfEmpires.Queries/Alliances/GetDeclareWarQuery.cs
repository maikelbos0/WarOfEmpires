using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetDeclareWarQuery : IQuery<DeclareWarModel> {
        public int AllianceId { get; }

        public GetDeclareWarQuery(int allianceId) {
            AllianceId = allianceId;
        }
    }
}
