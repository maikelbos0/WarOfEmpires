using WarOfEmpires.Models.Empires;

namespace WarOfEmpires.Queries.Empires {
    public sealed class GetResourceHeaderQuery : IQuery<ResourceHeaderViewModel> {
        public string Email { get; }

        public GetResourceHeaderQuery(string email) {
            Email = email;
        }
    }
}