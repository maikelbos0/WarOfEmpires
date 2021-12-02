using System.Collections.Generic;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Alliances {
    public class Role : Entity {
        public virtual Alliance Alliance { get; protected set; }
        public virtual ICollection<Player> Players { get; protected set; } = new List<Player>();
        public virtual string Name { get; protected set; }
        public virtual bool CanInvite { get; protected set; }
        public virtual bool CanManageRoles { get; protected set; }
        public virtual bool CanDeleteChatMessages { get; protected set; }
        public virtual bool CanKickMembers { get; protected set; }
        public virtual bool CanManageNonAggressionPacts { get; protected set; }
        public virtual bool CanManageWars { get; protected set; }
        public virtual bool CanBank { get; protected set; }

        protected Role() {
        }

        public Role(Alliance alliance, string name, bool canInvite, bool canManageRoles, bool canDeleteChatMessages, bool canKickMembers, bool canManageNonAggressionPacts, bool canManageWars, bool canBank) {
            Alliance = alliance;
            Name = name;
            CanInvite = canInvite;
            CanManageRoles = canManageRoles;
            CanDeleteChatMessages = canDeleteChatMessages;
            CanKickMembers = canKickMembers;
            CanManageNonAggressionPacts = canManageNonAggressionPacts;
            CanManageWars = canManageWars;
            CanBank = canBank;
        }
    }
}