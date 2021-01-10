using System.Collections.Generic;
using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetReceivedNonAggressionPactRequestsQuery : IQuery<IEnumerable<ReceivedNonAggressionPactRequestViewModel>> {
        public string Email { get; set; }

        public GetReceivedNonAggressionPactRequestsQuery(string email) {
            Email = email;
        }
    }
}
