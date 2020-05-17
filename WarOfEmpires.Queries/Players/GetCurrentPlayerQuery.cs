using WarOfEmpires.Models.Players;

namespace WarOfEmpires.Queries.Players {
    public sealed class GetCurrentPlayerQuery : IQuery<CurrentPlayerViewModel> {
        public string Email { get; }

        public GetCurrentPlayerQuery(string email) {
            Email = email;
        }
    }
}