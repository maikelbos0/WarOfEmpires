using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetInvitesQuery : IQuery<InvitesViewModel> {
        public string Email { get; set; }

        public GetInvitesQuery(string email) {
            Email = email;
        }
    }
}