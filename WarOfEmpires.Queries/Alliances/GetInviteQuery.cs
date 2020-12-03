using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetInviteQuery : IQuery<InviteDetailsViewModel> {
        public string Email { get; set; }
        public int InviteId { get; }

        public GetInviteQuery(string email, int inviteId) {
            Email = email;
            InviteId = inviteId;
        }
    }
}