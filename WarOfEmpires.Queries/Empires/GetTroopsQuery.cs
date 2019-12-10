using WarOfEmpires.Models.Empires;

namespace WarOfEmpires.Queries.Empires {
    public sealed class GetTroopsQuery : IQuery<TroopModel> {
        public string Email { get; }

        public GetTroopsQuery(string email) {
            Email = email;
        }
    }
}
