using System.Collections.Generic;
using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetSentNonAggressionPactRequestsQuery : IQuery<IEnumerable<SentNonAggressionPactRequestViewModel>> {
        public string Email { get; set; }

        public GetSentNonAggressionPactRequestsQuery(string email) {
            Email = email;
        }
    }
}
