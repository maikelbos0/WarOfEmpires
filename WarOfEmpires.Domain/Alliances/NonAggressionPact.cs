using System.Collections.Generic;

namespace WarOfEmpires.Domain.Alliances {
    public class NonAggressionPact {
        public virtual ICollection<Alliance> Alliances { get; set; } = new List<Alliance>();

        public NonAggressionPact() {
        }

        public void Dissolve() {
            foreach (var alliance in Alliances) {
                alliance.NonAggressionPacts.Remove(this);
            }
        }
    }
}
