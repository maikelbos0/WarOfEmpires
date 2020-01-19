using WarOfEmpires.Models.Empires;

namespace WarOfEmpires.Queries.Empires {
    public class GetSiegeQuery : IQuery<SiegeModel> {
        public string Email { get; }

        public GetSiegeQuery(string email) {
            Email = email;
        }
    }
}