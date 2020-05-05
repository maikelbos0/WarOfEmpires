using WarOfEmpires.Models.Players;

namespace WarOfEmpires.Queries.Players {
    public sealed class GetCurrentPlayerQuery : IQuery<CurrentPlayerViewModel> {
        public string Email { get; private set; }

        public GetCurrentPlayerQuery(string email) {
            Email = email;
        }
    }
}