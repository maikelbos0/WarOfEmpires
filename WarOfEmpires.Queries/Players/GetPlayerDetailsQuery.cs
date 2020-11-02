using WarOfEmpires.Models.Players;

namespace WarOfEmpires.Queries.Players {
    public sealed class GetPlayerDetailsQuery : IQuery<PlayerDetailsViewModel> {
        public string Email { get; }
        public int Id { get; }

        public GetPlayerDetailsQuery(string email, int id) {
            Email = email;
            Id = id;
        }
    }
}