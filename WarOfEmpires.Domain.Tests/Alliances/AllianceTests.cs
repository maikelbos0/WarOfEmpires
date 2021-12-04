using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Alliances {
    [TestClass]
    public sealed class AllianceTests {
        [TestMethod]
        public void Alliance_New_Adds_Leader_As_Member() {
            var leader = new Player(1, "Leader", Race.Elves);
            var alliance = new Alliance(leader, "TEST", "The Test");

            alliance.Members.Should().BeEquivalentTo(new[] { leader });
        }

        [TestMethod]
        public void Alliance_Leave_Succeeds() {
            var leader = new Player(1, "Leader", Race.Elves);
            var member = new Player(2, "Member", Race.Elves);
            var alliance = new Alliance(leader, "TEST", "The Test");

            alliance.Members.Add(member);
            member.HasNewChatMessages = true;

            alliance.Leave(member);

            alliance.Members.Should().BeEquivalentTo(new[] { leader });
            member.HasNewChatMessages.Should().BeFalse();
            alliance.ChatMessages.Should().HaveCount(1);
            alliance.ChatMessages.Single().Player.Should().BeNull();
            alliance.ChatMessages.Single().Message.Should().Be("Member has left our alliance.");
        }

        [TestMethod]
        public void Alliance_Kick_Succeeds() {
            var leader = new Player(1, "Leader", Race.Elves);
            var member = new Player(2, "Member", Race.Elves);
            var alliance = new Alliance(leader, "TEST", "The Test");

            alliance.Members.Add(member);
            member.HasNewChatMessages = true;

            alliance.Kick(member);

            alliance.Members.Should().BeEquivalentTo(new[] { leader });
            member.HasNewChatMessages.Should().BeFalse();
            alliance.ChatMessages.Should().HaveCount(1);
            alliance.ChatMessages.Single().Player.Should().BeNull();
            alliance.ChatMessages.Single().Message.Should().Be("Member has been kicked from our alliance.");
        }

        [TestMethod]
        public void Alliance_SendInvite_Succeeds() {
            var leader = new Player(1, "Leader", Race.Elves);
            var player = new Player(2, "Player", Race.Elves);
            var alliance = new Alliance(leader, "TEST", "The Test");

            alliance.SendInvite(player, "The subject", "A body");

            alliance.Invites.Should().HaveCount(1);
            alliance.Invites.Single().Player.Should().Be(player);
            alliance.Invites.Single().Alliance.Should().Be(alliance);
            alliance.Invites.Single().Subject.Should().Be("The subject");
            alliance.Invites.Single().Body.Should().Be("A body");
            alliance.Invites.Single().IsRead.Should().Be(false);
            alliance.Invites.Single().Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void Alliance_AcceptInvite_Succeeds() {
            var leader = new Player(1, "Leader", Race.Elves);
            var player = new Player(2, "New Guy", Race.Elves);
            var alliance = new Alliance(leader, "TEST", "The Test");
            var invite = new Invite(alliance, player, null, null);

            alliance.Invites.Add(invite);

            alliance.AcceptInvite(invite);

            alliance.Members.Should().BeEquivalentTo(new[] { leader, player });
            alliance.Invites.Should().BeEmpty();
            alliance.ChatMessages.Should().HaveCount(1);
            alliance.ChatMessages.Single().Player.Should().BeNull();
            alliance.ChatMessages.Single().Message.Should().Be("New Guy has joined our alliance.");
        }

        [TestMethod]
        public void Alliance_RemoveInvite_Succeeds() {
            var leader = new Player(1, "Leader", Race.Elves);
            var player = new Player(2, "Player", Race.Elves);
            var alliance = new Alliance(leader, "TEST", "The Test");
            var invite = new Invite(alliance, player, null, null);

            alliance.Invites.Add(invite);

            alliance.RemoveInvite(invite);

            alliance.Invites.Should().BeEmpty();
        }

        [TestMethod]
        public void Alliance_PostChatMessage_Succeeds() {
            var leader = new Player(1, "Leader", Race.Elves);
            var alliance = new Alliance(leader, "TEST", "The Test");

            alliance.PostChatMessage(leader, "Test message");

            alliance.ChatMessages.Should().HaveCount(1);
            alliance.ChatMessages.Single().Player.Should().Be(leader);
            alliance.ChatMessages.Single().Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            alliance.ChatMessages.Single().Message.Should().Be("Test message");
        }

        [TestMethod]
        public void Alliance_PostChatMessage_Sets_HasNewChatMessages() {
            var leader = new Player(1, "Leader", Race.Elves);
            var member1 = new Player(2, "Member 1", Race.Elves);
            var member2 = new Player(3, "Member 2", Race.Elves);
            var alliance = new Alliance(leader, "TEST", "The Test");

            alliance.Members.Add(member1);
            alliance.Members.Add(member2);

            alliance.PostChatMessage(leader, "Test message");

            leader.HasNewChatMessages.Should().BeFalse();
            member1.HasNewChatMessages.Should().BeTrue();
            member2.HasNewChatMessages.Should().BeTrue();
        }

        [TestMethod]
        public void Alliance_PostChatMessage__Without_Player_Succeeds() {
            var leader = new Player(1, "Leader", Race.Elves);
            var alliance = new Alliance(leader, "TEST", "The Test");

            alliance.PostChatMessage("Test message");

            alliance.ChatMessages.Should().HaveCount(1);
            alliance.ChatMessages.Single().Player.Should().BeNull();
            alliance.ChatMessages.Single().Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            alliance.ChatMessages.Single().Message.Should().Be("Test message");
            leader.HasNewChatMessages.Should().BeTrue();
        }

        [TestMethod]
        public void Alliance_DeleteChatMessage_Succeeds() {
            var leader = new Player(1, "Leader", Race.Elves);
            var alliance = new Alliance(leader, "TEST", "The Test");
            var chatMessage = new ChatMessage(leader, "Test message");

            alliance.ChatMessages.Add(chatMessage);

            alliance.DeleteChatMessage(chatMessage);

            alliance.ChatMessages.Should().HaveCount(0);
        }

        [TestMethod]
        public void Alliance_CreateRole_Succeeds() {
            var leader = new Player(1, "Leader", Race.Elves);
            var alliance = new Alliance(leader, "TEST", "The Test");

            alliance.CreateRole("Testrole", true, true, true, true, true, true, true);

            alliance.Roles.Should().HaveCount(1);
            alliance.Roles.Single().Alliance.Should().Be(alliance);
            alliance.Roles.Single().Name.Should().Be("Testrole");
            alliance.Roles.Single().CanInvite.Should().BeTrue();
            alliance.Roles.Single().CanManageRoles.Should().BeTrue();
            alliance.Roles.Single().CanDeleteChatMessages.Should().BeTrue();
            alliance.Roles.Single().CanKickMembers.Should().BeTrue();
            alliance.Roles.Single().CanManageNonAggressionPacts.Should().BeTrue();
            alliance.Roles.Single().CanManageWars.Should().BeTrue();
            alliance.Roles.Single().CanBank.Should().BeTrue();
        }

        [TestMethod]
        public void Alliance_DeleteRole_Succeeds() {
            var leader = new Player(1, "Leader", Race.Elves);
            var alliance = new Alliance(leader, "TEST", "The Test");
            var role = new Role(alliance, "Testrole", false, false, false, false, false, false, false);

            role.Players.Add(leader);
            alliance.Roles.Add(role);

            alliance.DeleteRole(role);

            alliance.Roles.Should().HaveCount(0);
            role.Players.Should().HaveCount(0);
        }

        [TestMethod]
        public void Alliance_SetRole_Succeeds() {
            var leader = new Player(1, "Leader", Race.Elves);
            var alliance = new Alliance(leader, "TEST", "The Test");
            var role = new Role(alliance, "Testrole", false, false, false, false, false, false, false);

            alliance.Roles.Add(role);

            alliance.SetRole(leader, role);

            role.Players.Should().BeEquivalentTo(new[] { leader });
        }

        [TestMethod]
        public void Alliance_ClearRole_Succeeds() {
            var leader = new Player(1, "Leader", Race.Elves);
            var member = new Player(2, "Member", Race.Elves);
            var alliance = new Alliance(leader, "TEST", "The Test");
            var role = new Role(alliance, "Testrole", false, false, false, false, false, false, false);

            role.Players.Add(leader);
            role.Players.Add(member);
            alliance.Roles.Add(role);

            alliance.ClearRole(leader);
            role.Players.Should().BeEquivalentTo(new[] { member });
        }

        [TestMethod]
        public void Alliance_TransferLeadership_Succeeds() {
            var oldLeader = new Player(1, "Old leader", Race.Elves);
            var newLeader = new Player(2, "New leader", Race.Elves);
            var alliance = new Alliance(oldLeader, "TEST", "The Test");

            alliance.Members.Add(newLeader);

            alliance.TransferLeadership(newLeader);

            alliance.Leader.Should().Be(newLeader);
            alliance.ChatMessages.Should().HaveCount(1);
            alliance.ChatMessages.Single().Player.Should().BeNull();
            alliance.ChatMessages.Single().Message.Should().Be("Old leader has transferred leadership to New leader.");
        }

        [TestMethod]
        public void Alliance_Disband_Succeeds() {
            var leader = new Player(1, "Leader", Race.Elves) { HasNewChatMessages = true };
            var member = new Player(2, "Member", Race.Elves) { HasNewChatMessages = true };
            var alliance = new Alliance(leader, "TEST", "The Test");
            var role = new Role(alliance, "Testrole", false, false, false, false, false, false, false);

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
            var senderAlliance = new Alliance(new Player(1, "Sender", Race.Elves), "SEND", "The Senders");
            var recipientAlliance = new Alliance(new Player(2, "Recipient", Race.Elves), "RECV", "The Recipients");

            senderAlliance.SendNonAggressionPactRequest(recipientAlliance);

            senderAlliance.SentNonAggressionPactRequests.Should().HaveCount(1);
            recipientAlliance.ReceivedNonAggressionPactRequests.Should().HaveCount(1);

            senderAlliance.SentNonAggressionPactRequests.Single().Sender.Should().Be(senderAlliance);
            senderAlliance.SentNonAggressionPactRequests.Single().Recipient.Should().Be(recipientAlliance);
        }

        [TestMethod]
        public void Alliance_DeclareWar_Succeeds() {
            var declaringAlliance = new Alliance(new Player(1, "Sender", Race.Elves), "SEND", "The Senders");
            var targetAlliance = new Alliance(new Player(2, "Target", Race.Elves), "TRGT", "The Targets");

            declaringAlliance.DeclareWar(targetAlliance);

            declaringAlliance.Wars.Should().HaveCount(1);
            targetAlliance.Wars.Should().HaveCount(1);
            declaringAlliance.Wars.Single().Should().Be(targetAlliance.Wars.Single());
            declaringAlliance.Wars.Single().Alliances.Should().BeEquivalentTo(new[] { declaringAlliance, targetAlliance });
            targetAlliance.ChatMessages.Should().HaveCount(1);
            targetAlliance.ChatMessages.Single().Player.Should().BeNull();
            targetAlliance.ChatMessages.Single().Message.Should().Be("The Senders have declared war on you.");
            declaringAlliance.ChatMessages.Should().HaveCount(1);
            declaringAlliance.ChatMessages.Single().Player.Should().BeNull();
            declaringAlliance.ChatMessages.Single().Message.Should().Be("You have declared war on The Targets.");
        }

        [TestMethod]
        public void Alliance_Update_Succeeds() {
            var alliance = new Alliance(new Player(1, "Leader", Race.Elves), "ALLY", "The Alliance");

            alliance.Update("NEW", "The Reborn");

            alliance.Code.Should().Be("NEW");
            alliance.Name.Should().Be("The Reborn");
        }

        [TestMethod]
        public void Alliance_Bank_Succeeds() {
            var player = Substitute.For<Player>();
            var alliance = new Alliance(player, "ALLY", "The Alliance");
            var previousBankTurns = alliance.BankTurns;

            alliance.Bank(player, 0.69, new Resources(5000, 4000, 3000, 2000, 1000));

            player.Received().SpendResources(new Resources(5000, 4000, 3000, 2000, 1000));
            alliance.BankTurns.Should().Be(previousBankTurns - 1);
            alliance.BankedResources.Should().Be(new Resources(5000, 4000, 3000, 2000, 1000) * 0.69M);
        }

        [TestMethod]
        public void Alliance_Bank_Succeeds_Minimum_Tax() {
            var player = Substitute.For<Player>();
            var alliance = new Alliance(player, "ALLY", "The Alliance");
            var previousBankTurns = alliance.BankTurns;

            alliance.Bank(player, 0.71, new Resources(5000, 4000, 3000, 2000, 1000));

            player.Received().SpendResources(new Resources(5000, 4000, 3000, 2000, 1000));
            alliance.BankTurns.Should().Be(previousBankTurns - 1);
            alliance.BankedResources.Should().Be(new Resources(5000, 4000, 3000, 2000, 1000) * 0.7M);
        }

        [TestMethod]
        public void Alliance_AddBankTurn_Adds_BankTurn() {
            var alliance = new Alliance(new Player(1, "Leader", Race.Elves), "ALLY", "The Alliance");
            var previousBankTurns = alliance.BankTurns;

            alliance.AddBankTurn();

            alliance.BankTurns.Should().Be(previousBankTurns + 1);
        }

        [TestMethod]
        public void Alliance_Reset_Succeeds() {
            var alliance = new Alliance(new Player(1, "Leader", Race.Elves), "ALLY", "The Alliance");

            typeof(Alliance).GetProperty(nameof(Alliance.BankedResources)).SetValue(alliance, new Resources(1000000, 10000, 100000, 100000, 100000));
            typeof(Alliance).GetProperty(nameof(Alliance.BankTurns)).SetValue(alliance, 20);

            alliance.Reset();

            alliance.BankedResources.Should().Be(new Resources());
            alliance.BankTurns.Should().Be(12);
        }
    }
}