using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetInviteQuery : IQuery<InviteDetailsViewModel> {
        public string Email { get; set; }
        public string InviteId { get; }

        public GetInviteQuery(string email, string inviteId) {
            Email = email;
            InviteId = inviteId;
        }
    }
}