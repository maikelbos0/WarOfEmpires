using System.Collections.Generic;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Alliances {
    public class Alliance : Entity {
        public virtual Player Leader { get; protected set; }
        public virtual string Code { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual ICollection<Player> Members { get; protected set; } = new List<Player>();
        public virtual ICollection<Invite> Invites { get; protected set; } = new List<Invite>();

        protected Alliance() {
        }

        public Alliance(Player leader, string code, string name) {
            Leader = leader;
            Code = code;
            Name = name;
        }

        public virtual void AddMember(Player member) {
            Members.Add(member);
        }
    }
}