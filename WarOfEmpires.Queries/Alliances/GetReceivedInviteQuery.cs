using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetReceivedInviteQuery : IQuery<ReceivedInviteDetailsViewModel> {
        public string Email { get; set; }
        public int InviteId { get; }

        public GetReceivedInviteQuery(string email, int inviteId) {
            Email = email;
            InviteId = inviteId;
        }
    }
}