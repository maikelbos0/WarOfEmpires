using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetAllianceDetailsQuery : IQuery<AllianceDetailsViewModel> {
        public string Email { get; set; }
        public int Id { get; }

        public GetAllianceDetailsQuery(string email, int id) {
            Id = id;
            Email = email;
        }
    }
}