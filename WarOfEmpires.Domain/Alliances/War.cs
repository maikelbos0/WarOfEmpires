using System.Collections.Generic;
using System.Linq;

namespace WarOfEmpires.Domain.Alliances {
    public class War : Entity {
        public virtual ICollection<Alliance> Alliances { get; set; } = new List<Alliance>();
        public virtual ICollection<Alliance> PeaceDeclarations { get; set; } = new List<Alliance>();

        public virtual void DeclarePeace(Alliance alliance) {
            PeaceDeclarations.Add(alliance);

            if (!Alliances.Except(PeaceDeclarations).Any()) {
                var message = $"The war between {string.Join(" and ", Alliances.Select(a => a.Name))} has ended.";

                foreach (var peaceAlliance in Alliances.ToList()) {
                    peaceAlliance.PostChatMessage(message);
                    peaceAlliance.Wars.Remove(this);
                }
            }
        }

        public virtual void CancelPeaceDeclaration(Alliance alliance) {
            PeaceDeclarations.Remove(alliance);
        }
    }
}
