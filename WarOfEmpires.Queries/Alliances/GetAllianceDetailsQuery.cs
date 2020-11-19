using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetAllianceDetailsQuery : IQuery<AllianceDetailsViewModel> {
        public int Id { get; }

        public GetAllianceDetailsQuery(int id) {
            Id = id;
        }
    }
}