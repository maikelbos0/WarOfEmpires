using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetReceivedInviteQuery : IQuery<ReceivedInviteDetailsViewModel> {
        public string Email { get; set; }
        public string InviteId { get; }

        public GetReceivedInviteQuery(string email, string inviteId) {
            Email = email;
            InviteId = inviteId;
        }
    }
}