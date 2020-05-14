using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetCurrentAllianceQuery : IQuery<AllianceDetailsViewModel> {
        public string Email { get; set; }

        public GetCurrentAllianceQuery(string email) {
            Email = email;
        }
    }
}