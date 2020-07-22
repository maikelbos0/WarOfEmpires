using System.Collections.Generic;
using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetRolesQuery : IQuery<IEnumerable<RoleViewModel>> {
        public string Email { get; set; }

        public GetRolesQuery(string email) {
            Email = email;
        }
    }
}