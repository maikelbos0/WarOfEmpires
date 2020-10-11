using WarOfEmpires.Models.Players;

namespace WarOfEmpires.Queries.Players {
    public sealed class GetPlayerDetailsQuery : IQuery<PlayerDetailsViewModel> {
        public string Email { get; }
        public string Id { get; }

        public GetPlayerDetailsQuery(string email, string id) {
            Email = email;
            Id = id;
        }
    }
}