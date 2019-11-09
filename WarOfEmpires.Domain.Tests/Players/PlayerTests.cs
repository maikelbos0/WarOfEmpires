using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Players {
    [TestClass]
    public sealed class PlayerTests {
        [TestMethod]
        public void Player_Recruit_Adds_To_CurrentRecruitingEffort() {
            var player = new Player(0, "Test");
            player.RecruitsPerDay = 3;

            player.Recruit();

            player.CurrentRecruitingEffort.Should().Be(3);
        }

        [TestMethod]
        public void Player_Recruit_Adds_Peasants_When_Possible() {
            var player = new Player(0, "Test");
            player.Peasants = 0;
            player.CurrentRecruitingEffort = 23;
            player.RecruitsPerDay = 3;

            player.Recruit();

            player.Peasants.Should().Be(1);
        }

        [TestMethod]
        public void Player_Recruit_Adds_No_Peasants_When_Not_Possible() {
            var player = new Player(0, "Test");
            player.Peasants = 0;
            player.CurrentRecruitingEffort = 3;
            player.RecruitsPerDay = 3;

            player.Recruit();

            player.Peasants.Should().Be(0);
        }

        [TestMethod]
        public void Player_Recruit_Gives_Correct_Effort_Remainder_When_Adding_Peasants() {
            var player = new Player(0, "Test");
            player.Peasants = 0;
            player.CurrentRecruitingEffort = 23;
            player.RecruitsPerDay = 3;

            player.Recruit();

            player.CurrentRecruitingEffort.Should().Be(2);
        }
    }
}