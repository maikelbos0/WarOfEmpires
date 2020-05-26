using System.Collections.Generic;
using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetReceivedInvitesQuery : IQuery<IEnumerable<ReceivedInviteViewModel>> {
        public string Email { get; set; }

        public GetReceivedInvitesQuery(string email) {
            Email = email;
        }
    }
}