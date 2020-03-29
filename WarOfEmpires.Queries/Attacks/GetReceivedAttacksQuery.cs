using System.Collections.Generic;
using WarOfEmpires.Models.Attacks;

namespace WarOfEmpires.Queries.Attacks {
    public sealed class GetReceivedAttacksQuery : IQuery<IEnumerable<ReceivedAttackViewModel>> {
        public string Email { get; }

        public GetReceivedAttacksQuery(string email) {
            Email = email;
        }
    }
}