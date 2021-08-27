using WarOfEmpires.Models.Players;

namespace WarOfEmpires.Queries.Players {
    public sealed class GetPlayerHomeQuery : IQuery<PlayerHomeViewModel> {
        public string Email { get; }

        public GetPlayerHomeQuery(string email) {
            Email = email;
        }
    }
}
