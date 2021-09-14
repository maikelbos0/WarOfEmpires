using System.Collections.Generic;
using WarOfEmpires.Models.Empires;

namespace WarOfEmpires.Queries.Empires {
    public sealed class GetQueuedResearchQuery : IQuery<IEnumerable<QueuedResearchViewModel>> {
        public string Email { get; }

        public GetQueuedResearchQuery(string email) {
            Email = email;
        }
    }
}
