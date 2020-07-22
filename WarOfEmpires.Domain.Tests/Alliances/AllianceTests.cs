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
        public void Alliance_AddMember_Succeeds() {
            var member = new Player(1, "Member");
            var player = new Player(2, "Player");
            var alliance = new Alliance(member, "TEST", "The Test");

            alliance.AddMember(member);
            alliance.AddMember(player);

            alliance.Members.Should().BeEquivalentTo(new[] { member, player });
        }

        [TestMethod]
        public void Alliance_PostChatMessage_Succeeds() {
            var member = new Player(1, "Member");
            var alliance = new Alliance(member, "TEST", "The Test");

            alliance.PostChatMessage(member, "Test message");

            alliance.ChatMessages.Should().HaveCount(1);
            alliance.ChatMessages.First().Player.Should().Be(member);
            alliance.ChatMessages.First().Date.Should().BeCloseTo(DateTime.UtcNow, 1000);
            alliance.ChatMessages.First().Message.Should().Be("Test message");
        }

        [TestMethod]
        public void Alliance_CreateRole_Succeeds() {
            var member = new Player(1, "Member");
            var alliance = new Alliance(member, "TEST", "The Test");

            alliance.CreateRole("Testrole");

            alliance.Roles.Should().HaveCount(1);
            alliance.Roles.First().Alliance.Should().Be(alliance);
            alliance.Roles.First().Name.Should().Be("Testrole");
        }

        [TestMethod]
        public void Alliance_DeleteRole_Succeeds() {
            var member = new Player(1, "Member");
            var alliance = new Alliance(member, "TEST", "The Test");
            var role = new Role(alliance, "Testrole");

            role.Players.Add(member);
            alliance.Roles.Add(role);

            alliance.DeleteRole(role);

            alliance.Roles.Should().HaveCount(0);
            role.Players.Should().HaveCount(0);
        }

        [TestMethod]
        public void Alliance_SetRole_Succeeds() {
            var member = new Player(1, "Member 1");
            var alliance = new Alliance(member, "TEST", "The Test");
            var role = new Role(alliance, "Testrole");

            alliance.Roles.Add(role);

            alliance.SetRole(member, role);

            role.Players.Should().BeEquivalentTo(member);
        }

        [TestMethod]
        public void Alliance_ClearRole_Succeeds() {
            var member1 = new Player(1, "Member 1");
            var member2 = new Player(2, "Member 2");
            var alliance = new Alliance(member1, "TEST", "The Test");
            var role = new Role(alliance, "Testrole");

            role.Players.Add(member1);
            role.Players.Add(member2);
            alliance.Roles.Add(role);

            alliance.ClearRole(member1);
            role.Players.Should().BeEquivalentTo(member2);
        }
    }
}