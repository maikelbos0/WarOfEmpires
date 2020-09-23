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
            var leader = new Player(1, "Member");
            var alliance = new Alliance(leader, "TEST", "The Test");

            alliance.Members.Should().BeEquivalentTo(new[] { leader });
        }

        [TestMethod]
        public void Alliance_AddMember_Succeeds() {
            var leader = new Player(1, "Member");
            var player = new Player(2, "Player");
            var alliance = new Alliance(leader, "TEST", "The Test");

            alliance.AddMember(player);

            alliance.Members.Should().BeEquivalentTo(new[] { leader, player });
        }

        [TestMethod]
        public void Alliance_SendInvite_Succeeds() {
            var member = new Player(1, "Member");
            var player = new Player(2, "Player");
            var alliance = new Alliance(member, "TEST", "The Test");

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
            var leader = new Player(1, "Member");
            var player = new Player(2, "Player");
            var alliance = new Alliance(leader, "TEST", "The Test");
            var invite = new Invite(alliance, player, null, null);

            alliance.Invites.Add(invite);

            alliance.AcceptInvite(invite);

            alliance.Members.Should().BeEquivalentTo(new[] { leader, player });
            alliance.Invites.Should().BeEmpty();
            player.Invites.Should().BeEmpty();
        }

        [TestMethod]
        public void Alliance_RemoveInvite_Succeeds() {
            var leader = new Player(1, "Member");
            var player = new Player(2, "Player");
            var alliance = new Alliance(leader, "TEST", "The Test");
            var invite = new Invite(alliance, player, null, null);

            alliance.Invites.Add(invite);

            alliance.RemoveInvite(invite);

            alliance.Invites.Should().BeEmpty();
            player.Invites.Should().BeEmpty();
        }

        [TestMethod]
        public void Alliance_PostChatMessage_Succeeds() {
            var leader = new Player(1, "Member");
            var alliance = new Alliance(leader, "TEST", "The Test");

            alliance.PostChatMessage(leader, "Test message");

            alliance.ChatMessages.Should().HaveCount(1);
            alliance.ChatMessages.Single().Player.Should().Be(leader);
            alliance.ChatMessages.Single().Date.Should().BeCloseTo(DateTime.UtcNow, 1000);
            alliance.ChatMessages.Single().Message.Should().Be("Test message");
        }

        [TestMethod]
        public void Alliance_CreateRole_Succeeds() {
            var leader = new Player(1, "Member");
            var alliance = new Alliance(leader, "TEST", "The Test");

            alliance.CreateRole("Testrole", true);

            alliance.Roles.Should().HaveCount(1);
            alliance.Roles.Single().Alliance.Should().Be(alliance);
            alliance.Roles.Single().Name.Should().Be("Testrole");
            alliance.Roles.Single().CanInvite.Should().BeTrue();
        }

        [TestMethod]
        public void Alliance_DeleteRole_Succeeds() {
            var leader = new Player(1, "Member");
            var alliance = new Alliance(leader, "TEST", "The Test");
            var role = new Role(alliance, "Testrole", false);

            role.Players.Add(leader);
            alliance.Roles.Add(role);

            alliance.DeleteRole(role);

            alliance.Roles.Should().HaveCount(0);
            role.Players.Should().HaveCount(0);
        }

        [TestMethod]
        public void Alliance_SetRole_Succeeds() {
            var leader = new Player(1, "Member 1");
            var alliance = new Alliance(leader, "TEST", "The Test");
            var role = new Role(alliance, "Testrole", false);

            alliance.Roles.Add(role);

            alliance.SetRole(leader, role);

            role.Players.Should().BeEquivalentTo(leader);
        }

        [TestMethod]
        public void Alliance_ClearRole_Succeeds() {
            var leader = new Player(1, "Member 1");
            var member = new Player(2, "Member 2");
            var alliance = new Alliance(leader, "TEST", "The Test");
            var role = new Role(alliance, "Testrole", false);

            role.Players.Add(leader);
            role.Players.Add(member);
            alliance.Roles.Add(role);

            alliance.ClearRole(leader);
            role.Players.Should().BeEquivalentTo(member);
        }
    }
}