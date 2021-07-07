using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Alliances {
    public class Alliance : Entity {
        public virtual Player Leader { get; protected set; }
        public virtual string Code { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual ICollection<Player> Members { get; protected set; } = new List<Player>();
        public virtual ICollection<Role> Roles { get; protected set; } = new List<Role>();
        public virtual ICollection<Invite> Invites { get; protected set; } = new List<Invite>();
        public virtual ICollection<ChatMessage> ChatMessages { get; protected set; } = new List<ChatMessage>();
        public virtual ICollection<NonAggressionPactRequest> SentNonAggressionPactRequests { get; protected set; } = new List<NonAggressionPactRequest>();
        public virtual ICollection<NonAggressionPactRequest> ReceivedNonAggressionPactRequests { get; protected set; } = new List<NonAggressionPactRequest>();
        public virtual ICollection<NonAggressionPact> NonAggressionPacts { get; protected set; } = new List<NonAggressionPact>();
        public virtual ICollection<War> Wars { get; protected set; } = new List<War>();
        public virtual ICollection<War> PeaceDeclarations { get; protected set; } = new List<War>();

        protected Alliance() {
        }

        public Alliance(Player leader, string code, string name) {
            Leader = leader;
            Code = code;
            Name = name;
            Members.Add(leader);
        }

        public virtual void RemoveMember(Player member) {
            Members.Remove(member);
        }

        public virtual void SendInvite(Player player, string subject, string body) {
            Invites.Add(new Invite(this, player, subject, body));
        }

        public virtual void AcceptInvite(Invite invite) {
            Members.Add(invite.Player);
            Invites.Remove(invite);
        }

        public virtual void RemoveInvite(Invite invite) {
            Invites.Remove(invite);
        }

        public virtual void PostChatMessage(Player poster, string message) {
            ChatMessages.Add(new ChatMessage(poster, message));

            foreach (var member in Members.Where(m => m != poster)) {
                member.HasNewChatMessages = true;
            }
        }

        public virtual void DeleteChatMessage(ChatMessage chatMessage) {
            ChatMessages.Remove(chatMessage);
        }

        public virtual void CreateRole(string name, bool canInvite, bool canManageRoles, bool canDeleteChatMessages, bool canKickMembers, bool canManageNonAggressionPacts, bool canManageWars) {
            Roles.Add(new Role(this, name, canInvite, canManageRoles, canDeleteChatMessages, canKickMembers, canManageNonAggressionPacts, canManageWars));
        }

        public virtual void DeleteRole(Role role) {
            role.Players.Clear();
            Roles.Remove(role);
        }

        public virtual void SetRole(Player member, Role role) {
            role.Players.Add(member);
        }

        public virtual void ClearRole(Player member) {
            Roles.Single(r => r.Players.Contains(member)).Players.Remove(member);
        }

        public virtual void TransferLeadership(Player member) {
            Leader = member;
        }

        public virtual void Disband() {
            foreach (var role in Roles) {
                role.Players.Clear();
            }

            Members.Clear();
            Leader = null;
        }

        public virtual void SendNonAggressionPactRequest(Alliance recipient) {
            var request = new NonAggressionPactRequest(this, recipient);

            SentNonAggressionPactRequests.Add(request);
            recipient.ReceivedNonAggressionPactRequests.Add(request);
        }

        public virtual void DeclareWar(Alliance target) {
            var war = new War();

            war.Alliances.Add(this);
            war.Alliances.Add(target);
            Wars.Add(war);
            target.Wars.Add(war);
        }
    }
}