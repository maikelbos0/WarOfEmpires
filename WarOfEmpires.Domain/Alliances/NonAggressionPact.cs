using System.Collections.Generic;
using System.Linq;

namespace WarOfEmpires.Domain.Alliances {
    public class NonAggressionPact : Entity {
        public virtual ICollection<Alliance> Alliances { get; set; } = new List<Alliance>();

        public virtual void Dissolve() {
            var message = $"The non-aggression pact between {string.Join(" and ", Alliances.Select(a => a.Name))} has been dissolved.";

            foreach (var alliance in Alliances.ToList()) {
                alliance.PostChatMessage(message);
                alliance.NonAggressionPacts.Remove(this);
            }
        }
    }
}
