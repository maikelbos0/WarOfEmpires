using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetInvitePlayerQuery : IQuery<SendInviteModel> {
        public string PlayerId { get; }

        public GetInvitePlayerQuery(string playerId) {
            PlayerId = playerId;
        }
    }
}