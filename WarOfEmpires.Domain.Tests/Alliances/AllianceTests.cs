using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Alliances {
    [TestClass]
    public sealed class AllianceTests {
        [TestMethod]
        public void Alliance_New_Adds_Leader_As_Member() {
            var leader = new Player(1, "Leader");
            var alliance = new Alliance(leader, "TEST", "The Test");

            alliance.Members.Should().BeEquivalentTo(new[] { leader });
        }

        [TestMethod]
        public void Alliance_RemoveMember_Succeeds() {
            var leader = new Player(1, "Leader");
            var member = new Player(2, "Member");
            var alliance = new Alliance(leader, "TEST", "The Test");

            alliance.Members.Add(member);
            member.HasNewChatMessages = true;

            alliance.RemoveMember(member);

            alliance.Members.Should().BeEquivalentTo(new[] { leader });
            member.HasNewChatMessages.Should().BeFalse();
        }

        [TestMethod]
        public void Alliance_SendInvite_Succeeds() {
            var leader = new Player(1, "Leader");
            var player = new Player(2, "Player");
            var alliance = new Alliance(leader, "TEST", "The Test");

            alliance.SendInvite(player, "The subject", "A body");

            alliance.Invites.Should().HaveCount(1);
            alliance.Invites.Single().Player.Should().Be(player);
            alliance.Invites.Single().Alliance.Should().Be(alliance);
            alliance.Invites.Single().Subject.Should().Be("The subject");
            alliance.Invites.Single().Body.Should().Be("A body");
            alliance.Invites.Single().IsRead.Should().Be(false);
            alliance.Invites.Single().Date.Should().BeCloseTo(DateTime.UtcNow, 1000);
        }

        [TestMethod]
        public void Alliance_AcceptInvite_Succeeds() {
            var leader = new Player(1, "Leader");
            var player = new Player(2, "Player");
            var alliance = new Alliance(leader, "TEST", "The Test");
            var invite = new Invite(alliance, player, null, null);

            alliance.Invites.Add(invite);

            alliance.AcceptInvite(invite);

            alliance.Members.Should().BeEquivalentTo(new[] { leader, player });
            alliance.Invites.Should().BeEmpty();
        }

        [TestMethod]
        public void Alliance_RemoveInvite_Succeeds() {
            var leader = new Player(1, "Leader");
            var player = new Player(2, "Player");
            var alliance = new Alliance(leader, "TEST", "The Test");
            var invite = new Invite(alliance, player, null, null);

            alliance.Invites.Add(invite);

            alliance.RemoveInvite(invite);

            alliance.Invites.Should().BeEmpty();
        }

        [TestMethod]
        public void Alliance_PostChatMessage_Succeeds() {
            var leader = new Player(1, "Leader");
            var alliance = new Alliance(leader, "TEST", "The Test");

            alliance.PostChatMessage(leader, "Test message");

            alliance.ChatMessages.Should().HaveCount(1);
            alliance.ChatMessages.Single().Player.Should().Be(leader);
            alliance.ChatMessages.Single().Date.Should().BeCloseTo(DateTime.UtcNow, 1000);
            alliance.ChatMessages.Single().Message.Should().Be("Test message");
        }

        [TestMethod]
        public void Alliance_PostChatMessage_Sets_HasNewChatMessages() {
            var leader = new Player(1, "Leader");
            var member1 = new Player(2, "Member 1");
            var member2 = new Player(3, "Member 2");
            var alliance = new Alliance(leader, "TEST", "The Test");

            alliance.Members.Add(member1);
            alliance.Members.Add(member2);

            alliance.PostChatMessage(leader, "Test message");

            leader.HasNewChatMessages.Should().BeFalse();
            member1.HasNewChatMessages.Should().BeTrue();
            member2.HasNewChatMessages.Should().BeTrue();
        }

        [TestMethod]
        public void Alliance_DeleteChatMessage_Succeeds() {
            var leader = new Player(1, "Leader");
            var alliance = new Alliance(leader, "TEST", "The Test");
            var chatMessage = new ChatMessage(leader, "Test message");

            alliance.ChatMessages.Add(chatMessage);

            alliance.DeleteChatMessage(chatMessage);

            alliance.ChatMessages.Should().HaveCount(0);
        }

        [TestMethod]
        public void Alliance_CreateRole_Succeeds() {
            var leader = new Player(1, "Leader");
            var alliance = new Alliance(leader, "TEST", "The Test");

            alliance.CreateRole("Testrole", true, true, true, true, true, true);

            alliance.Roles.Should().HaveCount(1);
            alliance.Roles.Single().Alliance.Should().Be(alliance);
            alliance.Roles.Single().Name.Should().Be("Testrole");
            alliance.Roles.Single().CanInvite.Should().BeTrue();
            alliance.Roles.Single().CanManageRoles.Should().BeTrue();
            alliance.Roles.Single().CanDeleteChatMessages.Should().BeTrue();
            alliance.Roles.Single().CanKickMembers.Should().BeTrue();
            alliance.Roles.Single().CanManageNonAggressionPacts.Should().BeTrue();
            alliance.Roles.Single().CanManageWars.Should().BeTrue();
        }

        [TestMethod]
        public void Alliance_DeleteRole_Succeeds() {
            var leader = new Player(1, "Leader");
            var alliance = new Alliance(leader, "TEST", "The Test");
            var role = new Role(alliance, "Testrole", false, false, false, false, false, false);

            role.Players.Add(leader);
            alliance.Roles.Add(role);

            alliance.DeleteRole(role);

            alliance.Roles.Should().HaveCount(0);
            role.Players.Should().HaveCount(0);
        }

        [TestMethod]
        public void Alliance_SetRole_Succeeds() {
            var leader = new Player(1, "Leader");
            var alliance = new Alliance(leader, "TEST", "The Test");
            var role = new Role(alliance, "Testrole", false, false, false, false, false, false);

            alliance.Roles.Add(role);

            alliance.SetRole(leader, role);

            role.Players.Should().BeEquivalentTo(leader);
        }

        [TestMethod]
        public void Alliance_ClearRole_Succeeds() {
            var leader = new Player(1, "Leader");
            var member = new Player(2, "Member");
            var alliance = new Alliance(leader, "TEST", "The Test");
            var role = new Role(alliance, "Testrole", false, false, false, false, false, false);

            role.Players.Add(leader);
            role.Players.Add(member);
            alliance.Roles.Add(role);

            alliance.ClearRole(leader);
            role.Players.Should().BeEquivalentTo(member);
        }

        [TestMethod]
        public void Alliance_TransferLeadership_Succeeds() {
            var oldLeader = new Player(1, "Old leader");
            var newLeader = new Player(2, "New leader");
            var alliance = new Alliance(oldLeader, "TEST", "The Test");

            alliance.Members.Add(newLeader);

            alliance.TransferLeadership(newLeader);
            alliance.Leader.Should().Be(newLeader);
        }

        [TestMethod]
        public void Alliance_Disband_Succeeds() {
            var leader = new Player(1, "Leader") { HasNewChatMessages = true };
            var member = new Player(2, "Member") { HasNewChatMessages = true };
            var alliance = new Alliance(leader, "TEST", "The Test");
            var role = new Role(alliance, "Testrole", false, false, false, false, false, false);

            alliance.Members.Add(leader);
            alliance.Members.Add(member);

            alliance.Roles.Add(role);
            role.Players.Add(leader);
            role.Players.Add(member);

            alliance.Disband();

            alliance.Leader.Should().BeNull();
            alliance.Members.Should().BeEmpty();
            role.Players.Should().BeEmpty();
            leader.HasNewChatMessages.Should().BeFalse();
            member.HasNewChatMessages.Should().BeFalse();
        }

        [TestMethod]
        public void Alliance_SendNonAggressionPactRequest_Succeeds() {
            var senderAlliance = new Alliance(new Player(1, "Sender"), "SEND", "The Senders");
            var recipientAlliance = new Alliance(new Player(2, "Recipient"), "RECV", "The Recipients");

            senderAlliance.SendNonAggressionPactRequest(recipientAlliance);

            senderAlliance.SentNonAggressionPactRequests.Should().HaveCount(1);
            recipientAlliance.ReceivedNonAggressionPactRequests.Should().HaveCount(1);

            senderAlliance.SentNonAggressionPactRequests.Single().Sender.Should().Be(senderAlliance);
            senderAlliance.SentNonAggressionPactRequests.Single().Recipient.Should().Be(recipientAlliance);
        }

        [TestMethod]
        public void Alliance_DeclareWar_Succeeds() {
            var declaringAlliance = new Alliance(new Player(1, "Sender"), "SEND", "The Senders");
            var targetAlliance = new Alliance(new Player(2, "Target"), "TRGT", "The Targets");

            declaringAlliance.DeclareWar(targetAlliance);

            declaringAlliance.Wars.Should().HaveCount(1);
            targetAlliance.Wars.Should().HaveCount(1);
            declaringAlliance.Wars.Single().Should().Be(targetAlliance.Wars.Single());
            declaringAlliance.Wars.Single().Alliances.Should().BeEquivalentTo(declaringAlliance, targetAlliance);
        }
    }
}