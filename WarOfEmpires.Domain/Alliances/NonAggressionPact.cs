using System.Collections.Generic;
using System.Linq;

namespace WarOfEmpires.Domain.Alliances {
    public class NonAggressionPact : Entity {
        public virtual ICollection<Alliance> Alliances { get; set; } = new List<Alliance>();

        public virtual void Dissolve() {
            foreach (var alliance in Alliances.ToList()) {
                alliance.NonAggressionPacts.Remove(this);
            }
        }
    }
}
