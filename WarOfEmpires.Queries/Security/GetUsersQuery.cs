using System.Collections.Generic;
using WarOfEmpires.Models.Security;

namespace WarOfEmpires.Queries.Security {
    public sealed class GetUsersQuery : IQuery<IEnumerable<UserViewModel>> {
        public string DisplayName { get; }

        public GetUsersQuery(string displayName) {
            DisplayName = displayName;
        }
    }
}
