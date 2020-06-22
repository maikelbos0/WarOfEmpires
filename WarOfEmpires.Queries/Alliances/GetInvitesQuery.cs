using System.Collections.Generic;
using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetInvitesQuery : IQuery<IEnumerable<InviteViewModel>> {
        public string Email { get; set; }

        public GetInvitesQuery(string email) {
            Email = email;
        }
    }
}