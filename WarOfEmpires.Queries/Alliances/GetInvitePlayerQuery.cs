using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetInvitePlayerQuery : IQuery<SendInviteModel> {
        public int PlayerId { get; }

        public GetInvitePlayerQuery(int playerId) {
            PlayerId = playerId;
        }
    }
}