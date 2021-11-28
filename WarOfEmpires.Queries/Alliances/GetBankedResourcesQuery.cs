using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetBankedResourcesQuery : IQuery<BankedResourcesViewModel> {
        public string Email { get; set; }

        public GetBankedResourcesQuery(string email) {
            Email = email;
        }
    }
}
