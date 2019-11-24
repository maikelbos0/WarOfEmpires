using WarOfEmpires.Models.Empires;

namespace WarOfEmpires.Queries.Empires {
    public sealed class GetResourcesQuery : IQuery<ResourcesViewModel> {
        public string Email { get; }

        public GetResourcesQuery(string email) {
            Email = email;
        }
    }
}