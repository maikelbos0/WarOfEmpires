using System.Collections.Generic;
using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetNonAggressionPactsQuery : IQuery<IEnumerable<NonAggressionPactViewModel>> {
        public string Email { get; set; }

        public GetNonAggressionPactsQuery(string email) {
            Email = email;
        }
    }
}
