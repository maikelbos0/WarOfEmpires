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
    }
}