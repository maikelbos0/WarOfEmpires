﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Players {
    [TestClass]
    public sealed class PlayerTests {
        [TestMethod]
        public void Player_Recruit_Adds_To_CurrentRecruitingEffort() {
            var player = new Player(0, "Test");
            var previousRecruitingEffort = player.CurrentRecruitingEffort;

            player.Recruit();

            player.CurrentRecruitingEffort.Should().Be(previousRecruitingEffort + player.RecruitsPerDay);
        }

        [TestMethod]
        public void Player_Recruit_Adds_Peasants_When_Possible() {
            var player = new Player(0, "Test");
            var previousPeasants = player.Peasants;

            while (player.CurrentRecruitingEffort < 24) {
                player.Recruit();
            }

            player.Recruit();

            player.Peasants.Should().Be(previousPeasants + 1);
        }

        [TestMethod]
        public void Player_Recruit_Adds_No_Peasants_When_Not_Possible() {
            var player = new Player(0, "Test");
            var previousPeasants = player.Peasants;

            player.Recruit();

            player.Peasants.Should().Be(previousPeasants);
        }

        [TestMethod]
        public void Player_Recruit_Gives_Correct_Effort_Remainder_When_Adding_Peasants() {
            var player = new Player(0, "Test");
            var previousRecruitingEffort = player.CurrentRecruitingEffort;
            var previousPeasants = player.Peasants;

            // We don't have a way to increase recruiting yet
            typeof(Player).GetProperty(nameof(Player.RecruitsPerDay)).SetValue(player, 9);

            while (player.Peasants == previousPeasants) {
                player.Recruit();
            }

            player.CurrentRecruitingEffort.Should().Be(previousRecruitingEffort + 3 % 24);
        }

        [TestMethod]
        public void Player_TrainWorkers_Trains_Workers() {
            var player = new Player(0, "Test");
            var previousFarmers = player.Farmers;
            var previousWoodWorkers = player.WoodWorkers;
            var previousStoneMasons = player.StoneMasons;
            var previousOreMiners = player.OreMiners;

            player.TrainWorkers(1, 2, 4, 8);

            player.Farmers.Should().Be(previousFarmers + 1);
            player.WoodWorkers.Should().Be(previousWoodWorkers + 2);
            player.StoneMasons.Should().Be(previousStoneMasons + 4);
            player.OreMiners.Should().Be(previousOreMiners + 8);
        }

        [TestMethod]
        public void Player_TrainWorkers_Removes_Peasants() {
            var player = new Player(0, "Test");
            var previousPeasants = player.Peasants;

            player.TrainWorkers(1, 2, 4, 8);

            player.Peasants.Should().Be(previousPeasants - 15);
        }

        [TestMethod]
        public void Player_TrainWorkers_Removes_Gold() {
            var player = new Player(0, "Test");
            var previousGold = player.Gold;

            player.TrainWorkers(1, 2, 4, 8);

            player.Gold.Should().Be(previousGold - 15 * 250);
        }

        [TestMethod]
        public void Player_UntrainWorkers_Untrains_Workers() {
            var player = new Player(0, "Test");
            var previousFarmers = player.Farmers;
            var previousWoodWorkers = player.WoodWorkers;
            var previousStoneMasons = player.StoneMasons;
            var previousOreMiners = player.OreMiners;

            player.UntrainWorkers(8, 4, 2, 1);

            player.Farmers.Should().Be(previousFarmers - 8);
            player.WoodWorkers.Should().Be(previousWoodWorkers - 4);
            player.StoneMasons.Should().Be(previousStoneMasons - 2);
            player.OreMiners.Should().Be(previousOreMiners - 1);
        }

        [TestMethod]
        public void Player_UntrainWorkers_Adds_Peasants() {
            var player = new Player(0, "Test");
            var previousPeasants = player.Peasants;

            player.UntrainWorkers(8, 4, 2, 1);

            player.Peasants.Should().Be(previousPeasants + 15);
        }
    }
}