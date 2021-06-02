using System.Collections.Generic;

namespace WarOfEmpires.Domain.Alliances {
    public class War : Entity {
        public virtual ICollection<Alliance> Alliances { get; set; } = new List<Alliance>();
    }
}
