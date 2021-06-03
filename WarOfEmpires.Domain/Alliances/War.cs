using System.Collections.Generic;
using System.Linq;

namespace WarOfEmpires.Domain.Alliances {
    public class War : Entity {
        public virtual ICollection<Alliance> Alliances { get; set; } = new List<Alliance>();
        public virtual ICollection<Alliance> PeaceDeclarations { get; set; } = new List<Alliance>();

        public void DeclarePeace(Alliance alliance) {
            PeaceDeclarations.Add(alliance);

            if (!Alliances.Except(PeaceDeclarations).Any()) {
                foreach (var peaceAlliance in Alliances.ToList()) {
                    peaceAlliance.Wars.Remove(this);
                }
            }
        }
    }
}
