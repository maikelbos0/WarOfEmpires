using System.Collections.Generic;

namespace WarOfEmpires.Domain.Alliances {
    public class NonAggressionPact {
        public virtual ICollection<Alliance> Alliances { get; set; }

        public NonAggressionPact() {
        }
    }
}
