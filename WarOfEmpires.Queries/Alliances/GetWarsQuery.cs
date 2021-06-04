using System.Collections.Generic;
using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetWarsQuery : IQuery<IEnumerable<WarViewModel>> {
        public string Email { get; set; }

        public GetWarsQuery(string email) {
            Email = email;
        }
    }
}
