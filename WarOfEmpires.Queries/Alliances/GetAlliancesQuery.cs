using System.Collections.Generic;
using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetAlliancesQuery : IQuery<IEnumerable<AllianceViewModel>> {
        public string Email { get; }
        public string Code { get; }
        public string Name { get; }

        public GetAlliancesQuery(string email, string code, string name) {
            Email = email;
            Code = code;
            Name = name;
        }
    }
}