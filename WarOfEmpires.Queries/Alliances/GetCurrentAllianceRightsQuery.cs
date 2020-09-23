using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetCurrentAllianceRightsQuery : IQuery<CurrentAllianceRightsViewModel> {
        public string Email { get; set; }

        public GetCurrentAllianceRightsQuery(string email) {
            Email = email;
        }
    }
}