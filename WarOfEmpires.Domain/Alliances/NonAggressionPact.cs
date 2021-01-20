using System.Collections.Generic;

namespace WarOfEmpires.Domain.Alliances {
    public class NonAggressionPact : Entity {
        public virtual ICollection<Alliance> Alliances { get; set; } = new List<Alliance>();

        public NonAggressionPact() {
        }

        public virtual void Dissolve() {
            foreach (var alliance in Alliances) {
                alliance.NonAggressionPacts.Remove(this);
            }
        }
    }
}
