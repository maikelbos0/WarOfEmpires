using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Players {
    [TestClass]
    public sealed class PlayerTests {
        [TestMethod]
        public void Player_Recruit_Adds_To_CurrentRecruitingEffort() {
            var player = new Player(0, "Test");
            var previousRecruitingEffort = player.CurrentRecruitingEffort;

            player.Recruit();

            player.CurrentRecruitingEffort.Should().Be(previousRecruitingEffort + player.GetRecruitsPerDay());
        }

        [TestMethod]
        public void Player_Recruit_Adds_Peasants_When_Possible() {
            var player = new Player(0, "Test");
            var previousPeasants = player.Peasants;

            while (player.CurrentRecruitingEffort < 23) {
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

            player.Buildings.Add(new Building(player, BuildingType.Farm, 8));
            player.Buildings.Add(new Building(player, BuildingType.Lumberyard, 8));
            player.Buildings.Add(new Building(player, BuildingType.Quarry, 8));
            player.Buildings.Add(new Building(player, BuildingType.Mine, 8));

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
            var previousResources = player.Resources;

            player.TrainWorkers(1, 2, 4, 8);

            player.Resources.Should().Be(previousResources - 15 * Player.WorkerTrainingCost);
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

        [TestMethod]
        public void Player_GetTaxRate_Is_Correct() {
            var player = new Player(0, "Test") {
                Tax = 20
            };

            player.GetTaxRate().Should().Be(0.2m);
        }

        [TestMethod]
        public void Player_GetGoldPerTurn_Is_Correct() {
            var player = new Player(0, "Test") {
                Tax = 30
            };

            player.TrainWorkers(1, 2, 3, 4);

            player.GetGoldPerTurn().Should().Be(1500);
        }

        [TestMethod]
        public void Player_GetFoodPerTurn_Is_Correct() {
            var player = new Player(0, "Test") {
                Tax = 20
            };

            player.Buildings.Add(new Building(player, BuildingType.Farm, 4));
            player.TrainWorkers(1, 2, 3, 4);

            player.GetFoodPerTurn().Should().Be(32);
        }

        [TestMethod]
        public void Player_GetWoodPerTurn_Is_Correct() {
            var player = new Player(0, "Test") {
                Tax = 40
            };

            player.Buildings.Add(new Building(player, BuildingType.Lumberyard, 6));
            player.TrainWorkers(1, 2, 3, 4);

            player.GetWoodPerTurn().Should().Be(60);
        }

        [TestMethod]
        public void Player_GetStonePerTurn_Is_Correct() {
            var player = new Player(0, "Test") {
                Tax = 60
            };

            player.Buildings.Add(new Building(player, BuildingType.Quarry, 8));
            player.TrainWorkers(1, 2, 3, 4);

            player.GetStonePerTurn().Should().Be(72);
        }

        [TestMethod]
        public void Player_GetOrePerTurn_Is_Correct() {
            var player = new Player(0, "Test") {
                Tax = 80
            };

            player.Buildings.Add(new Building(player, BuildingType.Mine, 16));
            player.TrainWorkers(1, 2, 3, 4);

            player.GetOrePerTurn().Should().Be(80);
        }

        [TestMethod]
        public void Player_GatherResources_Succeeds() {
            var player = new Player(0, "Test");
            player.TrainWorkers(1, 2, 3, 4);

            var previousResources = player.Resources;

            player.Tax = 80;
            player.GatherResources();

            player.Resources.Should().Be(previousResources + new Resources(
                player.GetGoldPerTurn(),
                player.GetFoodPerTurn(),
                player.GetWoodPerTurn(),
                player.GetStonePerTurn(),
                player.GetOrePerTurn()
            ) - player.GetFoodCostPerTurn());
        }

        [TestMethod]
        public void Player_GatherResources_Does_Nothing_When_Out_Of_Food() {
            var player = new Player(0, "Test");
            player.TrainWorkers(1, 2, 3, 4);
            player.Tax = 85;

            while (player.Resources.Food >= player.GetFoodCostPerTurn().Food) {
                player.GatherResources();
            }

            var previousResources = player.Resources;

            player.GatherResources();

            player.Resources.Should().Be(previousResources - new Resources(food: previousResources.Food));
        }

        [TestMethod]
        public void Player_UpgradeBuilding_Succeeds_For_New_BuildingType() {
            var player = new Player(0, "Test");
            var buildingDefinition = BuildingDefinitionFactory.Get(BuildingType.Farm);

            player.TrainWorkers(0, 6, 3, 1);

            // Just get enough resources for anything
            for (var i = 0; i < 100000; i++) {
                player.GatherResources();
            }

            var previousResources = player.Resources;

            player.UpgradeBuilding(BuildingType.Farm);

            player.Buildings.Single().Level.Should().Be(1);
            player.Buildings.Single().Type.Should().Be(BuildingType.Farm);
            player.Resources.Should().Be(previousResources - buildingDefinition.GetNextLevelCost(0));
        }

        [TestMethod]
        public void Player_UpgradeBuilding_Succeeds_For_Existing_BuildingType() {
            var player = new Player(0, "Test");
            var buildingDefinition = BuildingDefinitionFactory.Get(BuildingType.Farm);

            player.TrainWorkers(0, 6, 3, 1);
            player.Buildings.Add(new Building(player, BuildingType.Farm, 1));

            // Just get enough resources for anything
            for (var i = 0; i < 100000; i++) {
                player.GatherResources();
            }

            var previousResources = player.Resources;

            player.UpgradeBuilding(BuildingType.Farm);

            player.Buildings.Single().Level.Should().Be(2);
            player.Resources.Should().Be(previousResources - buildingDefinition.GetNextLevelCost(1));
        }

        [TestMethod]
        public void Player_GetBuildingResourceMultiplier_Succeeds_For_Nonexistent_BuildingType() {
            var player = new Player(0, "Test");

            player.GetBuildingResourceMultiplier(BuildingType.Farm).Should().Be(1m);
        }

        [TestMethod]
        public void Player_GetBuildingResourceMultiplier_Succeeds_For_Existing_BuildingType() {
            var player = new Player(0, "Test");

            player.Buildings.Add(new Building(player, BuildingType.Farm, 3));

            player.GetBuildingResourceMultiplier(BuildingType.Farm).Should().Be(1.75m);
        }

        [TestMethod]
        public void Player_GetFoodCostPerTurn_Is_Correct_For_Peasants() {
            var player = new Player(0, "Test");

            player.GetFoodCostPerTurn().Should().Be(new Resources(food: 20));
        }

        [TestMethod]
        public void Player_GetRecruitsPerDay_Succeeds() {
            var player = new Player(0, "Test");

            player.Buildings.Add(new Building(player, BuildingType.Farm, 4));
            player.Buildings.Add(new Building(player, BuildingType.Lumberyard, 6));
            player.Buildings.Add(new Building(player, BuildingType.Quarry, 2));
            player.Buildings.Add(new Building(player, BuildingType.Mine, 2));

            player.GetRecruitsPerDay().Should().Be(5);
        }

        [TestMethod]
        public void Player_GetRecruitsPerDay_Minimum_Is_1() {
            var player = new Player(0, "Test");

            player.GetRecruitsPerDay().Should().Be(1);
        }

        [TestMethod]
        public void Player_GetRecruitsPerDay_Maximum_Is_25() {
            var player = new Player(0, "Test");

            player.Buildings.Add(new Building(player, BuildingType.Farm, 100));
            player.Buildings.Add(new Building(player, BuildingType.Lumberyard, 100));
            player.Buildings.Add(new Building(player, BuildingType.Quarry, 100));
            player.Buildings.Add(new Building(player, BuildingType.Mine, 100));

            player.GetRecruitsPerDay().Should().Be(25);
        }

        [TestMethod]
        public void Player_GetTotalGoldSpentOnBuildings_Succeeds() {
            var player = new Player(0, "Test");

            player.Buildings.Add(new Building(player, BuildingType.Farm, 4));
            player.Buildings.Add(new Building(player, BuildingType.Lumberyard, 8));
            player.Buildings.Add(new Building(player, BuildingType.Quarry, 2));
            player.Buildings.Add(new Building(player, BuildingType.Mine, 2));

            player.GetTotalGoldSpentOnBuildings().Should().Be(1480000);

        }
    }
}