using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetAllianceHomeQuery : IQuery<AllianceHomeModel> {
        public string Email { get; set; }

        public GetAllianceHomeQuery(string email) {
            Email = email;
        }
    }
}