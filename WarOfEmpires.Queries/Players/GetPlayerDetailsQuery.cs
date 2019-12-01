using WarOfEmpires.Models.Players;

namespace WarOfEmpires.Queries.Players {
    public sealed class GetPlayerDetailsQuery : IQuery<PlayerDetailsViewModel> {
        public string Id { get; }

        public GetPlayerDetailsQuery(string id) {
            Id = id;
        }
    }
}