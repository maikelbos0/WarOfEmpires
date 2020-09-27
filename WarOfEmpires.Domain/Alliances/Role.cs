using System.Collections.Generic;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Alliances {
    public class Role : Entity {
        public virtual Alliance Alliance { get; protected set; }
        public virtual ICollection<Player> Players { get; protected set; } = new List<Player>();
        public virtual string Name { get; set; }
        // TODO move alliance rights to (flags) enum
        public virtual bool CanInvite { get; set; }

        protected Role() {
        }

        public Role(Alliance alliance, string name, bool canInvite) {
            Alliance = alliance;
            Name = name;
            CanInvite = canInvite;
        }
    }
}