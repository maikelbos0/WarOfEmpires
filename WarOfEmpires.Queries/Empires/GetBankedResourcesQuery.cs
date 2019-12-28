using WarOfEmpires.Models.Empires;

namespace WarOfEmpires.Queries.Empires {
    public sealed class GetBankedResourcesQuery : IQuery<BankedResourcesViewModel> {
        public string Email { get; }

        public GetBankedResourcesQuery(string email) {
            Email = email;
        }
    }
}