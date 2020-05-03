using System.Collections.Generic;
using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetAlliancesQuery : IQuery<IEnumerable<AllianceViewModel>> {
        public string Code { get; }
        public string Name { get; }

        public GetAlliancesQuery(string code, string name) {
            Code = code;
            Name = name;
        }
    }
}