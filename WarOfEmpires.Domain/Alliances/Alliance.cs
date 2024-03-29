﻿using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Alliances {
    public class Alliance : Entity {
        public const double MinimumBankTax = 0.3;

        public virtual Player Leader { get; protected set; }
        public virtual string Code { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual Resources BankedResources { get; protected set; } = new Resources();
        public virtual int BankTurns { get; protected set; } = 24;
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

        public virtual void Leave(Player member) {
            Members.Remove(member);
            member.HasNewChatMessages = false;
            PostChatMessage($"{member.DisplayName} has left our alliance.");
        }

        public virtual void Kick(Player member) {
            Members.Remove(member);
            member.HasNewChatMessages = false;
            PostChatMessage($"{member.DisplayName} has been kicked from our alliance.");
        }

        public virtual void SendInvite(Player player, string subject, string body) {
            Invites.Add(new Invite(this, player, subject, body));
        }

        public virtual void AcceptInvite(Invite invite) {
            Members.Add(invite.Player);
            Invites.Remove(invite);
            PostChatMessage($"{invite.Player.DisplayName} has joined our alliance.");
        }

        public virtual void RemoveInvite(Invite invite) {
            Invites.Remove(invite);
        }

        public virtual void PostChatMessage(string message) {
            PostChatMessage(null, message);
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

        public virtual void CreateRole(string name, bool canInvite, bool canManageRoles, bool canDeleteChatMessages, bool canKickMembers, bool canManageNonAggressionPacts, bool canManageWars, bool canBank) {
            Roles.Add(new Role(this, name, canInvite, canManageRoles, canDeleteChatMessages, canKickMembers, canManageNonAggressionPacts, canManageWars, canBank));
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
            PostChatMessage($"{Leader.DisplayName} has transferred leadership to {member.DisplayName}.");
            Leader = member;
        }

        public virtual void Disband() {
            foreach (var member in Members) {
                member.HasNewChatMessages = false;
            }

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
            PostChatMessage($"You have declared war on {target.Name}.");
            target.PostChatMessage($"{Name} have declared war on you.");
        }

        public virtual void Update(string code, string name) {
            Code = code;
            Name = name;
        }

        public virtual void Deposit(Player player, double ratio, Resources resources) {
            decimal tax = (decimal)Math.Max(1 - ratio, MinimumBankTax);

            BankTurns--;
            player.SpendResources(resources);
            BankedResources += resources * (1 - tax);
        }

        public virtual void AddBankTurn() {
            BankTurns++;
        }

        public virtual void Reset() {
            BankedResources = new Resources();
            BankTurns = 12;
        }

        public virtual void Withdraw(Player player, Resources resources) {
            BankTurns--;
            BankedResources -= resources;
            player.AddResources(resources);
        }
    }
}