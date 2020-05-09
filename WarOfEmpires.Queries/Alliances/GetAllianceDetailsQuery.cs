using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetAllianceDetailsQuery : IQuery<AllianceDetailsViewModel> {
        public string Id { get; }

        public GetAllianceDetailsQuery(string id) {
            Id = id;
        }
    }
}