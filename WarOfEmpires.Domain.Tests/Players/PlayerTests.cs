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
            player.Buildings.Add(new Building(player, BuildingType.Quarry, 7));
            player.Buildings.Add(new Building(player, BuildingType.Mine, 7));

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
        public void Player_GetFoodProduction_Is_Correct() {
            var player = new Player(0, "Test") {
                Tax = 20
            };

            player.Buildings.Add(new Building(player, BuildingType.Farm, 4));
            player.TrainWorkers(1, 2, 3, 4);

            player.GetFoodProduction().GetTotalProduction().Should().Be(32);
        }

        [TestMethod]
        public void Player_GetWoodProduction_Is_Correct() {
            var player = new Player(0, "Test") {
                Tax = 40
            };

            player.Buildings.Add(new Building(player, BuildingType.Lumberyard, 6));
            player.TrainWorkers(1, 2, 3, 4);

            player.GetWoodProduction().GetTotalProduction().Should().Be(60);
        }

        [TestMethod]
        public void Player_GetStoneProduction_Is_Correct() {
            var player = new Player(0, "Test") {
                Tax = 60
            };

            player.Buildings.Add(new Building(player, BuildingType.Quarry, 8));
            player.TrainWorkers(1, 2, 3, 4);

            player.GetStoneProduction().GetTotalProduction().Should().Be(72);
        }

        [TestMethod]
        public void Player_GetOreProduction_Is_Correct() {
            var player = new Player(0, "Test") {
                Tax = 80
            };

            player.Buildings.Add(new Building(player, BuildingType.Mine, 16));
            player.TrainWorkers(1, 2, 3, 4);

            player.GetOreProduction().GetTotalProduction().Should().Be(80);
        }

        [TestMethod]
        public void Player_ProcessTurn_Increases_Stamina() {
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.Stamina)).SetValue(player, 66);

            player.ProcessTurn();

            player.Stamina.Should().Be(67);
        }

        [TestMethod]
        public void Player_ProcessTurn_Adds_Resources() {
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(100000, 10000, 10000, 10000, 10000));
            player.TrainWorkers(1, 2, 1, 2);
            player.TrainTroops(0, 1, 0, 0, 0, 0);

            var previousResources = player.Resources;

            player.Tax = 80;
            player.ProcessTurn();

            player.Resources.Should().Be(previousResources + new Resources(
                player.GetGoldPerTurn(),
                player.GetFoodProduction().GetTotalProduction(),
                player.GetWoodProduction().GetTotalProduction(),
                player.GetStoneProduction().GetTotalProduction(),
                player.GetOreProduction().GetTotalProduction()
            ) - player.GetUpkeepPerTurn());
        }

        [TestMethod]
        public void Player_ProcessTurn_Adds_AttackTurns() {
            var player = new Player(0, "Test");

            var previousAttackTurns = player.AttackTurns;

            player.Tax = 80;
            player.ProcessTurn();

            player.AttackTurns.Should().Be(previousAttackTurns + 1);
        }

        [TestMethod]
        public void Player_ProcessTurn_Does_Not_Increase_Stamina_When_Full() {
            var player = new Player(0, "Test");

            player.ProcessTurn();

            player.Stamina.Should().Be(100);
        }

        [TestMethod]
        public void Player_ProcessTurn_Does_Not_Give_Resources_When_Out_Of_Food_Or_Gold() {
            var player = new Player(0, "Test");
            player.TrainWorkers(1, 2, 3, 4);
            player.Tax = 85;

            while (player.Resources.CanAfford(player.GetUpkeepPerTurn())) {
                player.ProcessTurn();
            }

            var previousResources = player.Resources;

            player.ProcessTurn();

            player.Resources.Should().Be(previousResources - new Resources(food: previousResources.Food));
            player.HasUpkeepRunOut.Should().BeTrue();
        }

        [TestMethod]
        public void Player_ProcessTurn_Gives_AttackTurns_When_Out_Of_Food_Or_Gold() {
            var player = new Player(0, "Test");

            while (player.Resources.CanAfford(player.GetUpkeepPerTurn())) {
                player.ProcessTurn();
            }

            var previousAttackTurns = player.AttackTurns;

            player.ProcessTurn();

            player.AttackTurns.Should().Be(previousAttackTurns + 1);
        }

        [TestMethod]
        public void Player_ProcessTurn_Increases_Stamina_When_Out_Of_Food_Or_Gold() {
            var player = new Player(0, "Test");

            while (player.Resources.CanAfford(player.GetUpkeepPerTurn())) {
                player.ProcessTurn();
            }

            typeof(Player).GetProperty(nameof(Player.Stamina)).SetValue(player, 66);

            player.ProcessTurn();

            player.Stamina.Should().Be(67);
        }

        [TestMethod]
        public void Player_ProcessTurn_Disbands_Mercenaries_When_Out_Of_Food_Or_Gold() {
            var player = new Player(0, "Test");
            player.TrainWorkers(1, 2, 1, 2);
            player.Tax = 85;

            while (player.Resources.CanAfford(player.GetUpkeepPerTurn())) {
                player.ProcessTurn();
            }

            player.TrainTroops(1, 1, 1, 1, 0, 0);

            player.ProcessTurn();
            player.Archers.Mercenaries.Should().Be(0);
            player.Cavalry.Mercenaries.Should().Be(0);
        }

        [TestMethod]
        public void Player_UpgradeBuilding_Succeeds_For_New_BuildingType() {
            var player = new Player(0, "Test");
            var buildingDefinition = BuildingDefinitionFactory.Get(BuildingType.Farm);
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(100000, 10000, 10000, 10000, 10000));

            player.TrainWorkers(0, 6, 3, 1);
            player.Buildings.Clear();

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
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(100000, 10000, 10000, 10000, 10000));

            player.TrainWorkers(0, 6, 3, 1);
            player.Buildings.Clear();
            player.Buildings.Add(new Building(player, BuildingType.Farm, 1));

            var previousResources = player.Resources;

            player.UpgradeBuilding(BuildingType.Farm);

            player.Buildings.Single().Level.Should().Be(2);
            player.Resources.Should().Be(previousResources - buildingDefinition.GetNextLevelCost(1));
        }

        [TestMethod]
        public void Player_GetBuildingBonusMultiplier_Succeeds_For_Nonexistent_BuildingType() {
            var player = new Player(0, "Test");

            player.GetBuildingBonusMultiplier(BuildingType.Farm).Should().Be(1m);
        }

        [TestMethod]
        public void Player_GetBuildingBonusMultiplier_Succeeds_For_Existing_BuildingType() {
            var player = new Player(0, "Test");

            player.Buildings.Add(new Building(player, BuildingType.Farm, 3));

            player.GetBuildingBonusMultiplier(BuildingType.Farm).Should().Be(1.75m);
        }

        [TestMethod]
        public void Player_GetUpkeepPerTurn_Succeeds() {
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(100000, 10000, 10000, 10000, 10000));
            player.TrainTroops(1, 1, 1, 1, 1, 1);

            player.GetUpkeepPerTurn().Should().Be(new Resources(food: 26, gold: 750));
        }

        [TestMethod]
        public void Player_GetTheoreticalRecruitsPerDay_Succeeds() {
            var player = new Player(0, "Test");

            player.Buildings.Add(new Building(player, BuildingType.Farm, 4));
            player.Buildings.Add(new Building(player, BuildingType.Lumberyard, 6));
            player.Buildings.Add(new Building(player, BuildingType.Quarry, 2));
            player.Buildings.Add(new Building(player, BuildingType.Mine, 2));
            player.Buildings.Add(new Building(player, BuildingType.Defences, 3));

            player.GetTheoreticalRecruitsPerDay().Should().Be(8);
        }

        [TestMethod]
        public void Player_GetTheoreticalRecruitsPerDay_Minimum_Is_1() {
            var player = new Player(0, "Test");

            player.GetTheoreticalRecruitsPerDay().Should().Be(1);
        }

        [TestMethod]
        public void Player_GetTheoreticalRecruitsPerDay_Maximum_Is_25() {
            var player = new Player(0, "Test");

            player.Buildings.Add(new Building(player, BuildingType.Farm, 15));
            player.Buildings.Add(new Building(player, BuildingType.Lumberyard, 15));
            player.Buildings.Add(new Building(player, BuildingType.Quarry, 15));
            player.Buildings.Add(new Building(player, BuildingType.Mine, 15));
            player.Buildings.Add(new Building(player, BuildingType.Defences, 15));

            player.GetTheoreticalRecruitsPerDay().Should().Be(25);
        }

        [TestMethod]
        public void Player_GetAvailableHousingCapacity_Succeeds() {
            var player = new Player(0, "Test");

            typeof(Player).GetProperty(nameof(Player.Archers)).SetValue(player, new Troops(19, 0));
            typeof(Player).GetProperty(nameof(Player.Peasants)).SetValue(player, 10);

            player.GetAvailableHousingCapacity().Should().Be(11);
        }

        [TestMethod]
        public void Player_GetAvailableHousingCapacity_Succeeds_For_Crowded_Barracks() {
            var player = new Player(0, "Test");

            typeof(Player).GetProperty(nameof(Player.Archers)).SetValue(player, new Troops(6, 1));
            typeof(Player).GetProperty(nameof(Player.Cavalry)).SetValue(player, new Troops(6, 1));
            typeof(Player).GetProperty(nameof(Player.Footmen)).SetValue(player, new Troops(6, 1));
            typeof(Player).GetProperty(nameof(Player.Peasants)).SetValue(player, 10);

            player.GetAvailableHousingCapacity().Should().Be(9);
        }

        [TestMethod]
        public void Player_GetAvailableHousingCapacity_Succeeds_For_Crowded_Huts() {
            var player = new Player(0, "Test");

            typeof(Player).GetProperty(nameof(Player.Peasants)).SetValue(player, 5);
            typeof(Player).GetProperty(nameof(Player.Farmers)).SetValue(player, 4);
            typeof(Player).GetProperty(nameof(Player.WoodWorkers)).SetValue(player, 4);
            typeof(Player).GetProperty(nameof(Player.StoneMasons)).SetValue(player, 4);
            typeof(Player).GetProperty(nameof(Player.OreMiners)).SetValue(player, 4);

            player.GetAvailableHousingCapacity().Should().Be(19);
        }

        [TestMethod]
        public void Player_GetRecruitsPerDay_Is_Maxed_By_Barracks_And_Huts_Capacity() {
            var player = new Player(0, "Test");

            player.Buildings.Add(new Building(player, BuildingType.Farm, 4));
            player.Buildings.Add(new Building(player, BuildingType.Lumberyard, 6));
            player.Buildings.Add(new Building(player, BuildingType.Quarry, 2));
            player.Buildings.Add(new Building(player, BuildingType.Mine, 2));
            player.Buildings.Add(new Building(player, BuildingType.Defences, 3));

            typeof(Player).GetProperty(nameof(Player.Archers)).SetValue(player, new Troops(19, 0));
            typeof(Player).GetProperty(nameof(Player.Peasants)).SetValue(player, 20);

            player.GetRecruitsPerDay().Should().Be(1);
        }

        [TestMethod]
        public void Player_GetTotalGoldSpentOnBuildings_Succeeds() {
            var player = new Player(0, "Test");

            player.Buildings.Add(new Building(player, BuildingType.Farm, 4));
            player.Buildings.Add(new Building(player, BuildingType.Lumberyard, 8));
            player.Buildings.Add(new Building(player, BuildingType.Quarry, 2));
            player.Buildings.Add(new Building(player, BuildingType.Mine, 2));

            player.GetTotalGoldSpentOnBuildings().Should().Be(1580000);
        }

        [TestMethod]
        public void Player_GetTotalGoldSpentOnBuildings_Ignores_Defences() {
            var player = new Player(0, "Test");

            player.Buildings.Add(new Building(player, BuildingType.Defences, 10));

            player.GetTotalGoldSpentOnBuildings().Should().Be(0);
        }

        [TestMethod]
        public void Player_TrainTroops_Trains_Troops() {
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(200000, 10000, 10000, 10000, 10000));

            player.TrainTroops(1, 4, 2, 5, 3, 6);

            player.Archers.Should().Be(new Troops(1, 4));
            player.Cavalry.Should().Be(new Troops(2, 5));
            player.Footmen.Should().Be(new Troops(3, 6));
        }

        [TestMethod]
        public void Player_TrainTroops_Removes_Peasants() {
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(200000, 10000, 10000, 10000, 10000));

            player.TrainTroops(1, 4, 2, 5, 3, 6);

            player.Peasants.Should().Be(4);
        }

        [TestMethod]
        public void Player_TrainTroops_Costs_Resources() {
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(200000, 10000, 10000, 10000, 10000));

            player.TrainTroops(1, 4, 2, 5, 3, 6);

            player.Resources.Should().Be(new Resources(95000, 10000, 7500, 10000, 3500));
        }

        [TestMethod]
        public void Player_UntrainTroops_Removes_Troops() {
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(200000, 10000, 10000, 10000, 10000));

            player.TrainTroops(2, 5, 3, 6, 4, 7);
            player.UntrainTroops(1, 4, 2, 5, 3, 6);

            player.Archers.Should().Be(new Troops(1, 1));
            player.Cavalry.Should().Be(new Troops(1, 1));
            player.Footmen.Should().Be(new Troops(1, 1));
        }

        [TestMethod]
        public void Player_UntrainTroops_Adds_Peasants() {
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(200000, 10000, 10000, 10000, 10000));

            player.TrainTroops(2, 5, 3, 6, 4, 7);
            player.UntrainTroops(1, 4, 2, 5, 3, 6);

            player.Peasants.Should().Be(7);
        }

        [TestMethod]
        public void Player_GetArcherInfo_Succeeds() {
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(1000000, 100000, 100000, 100000, 100000));

            player.UpgradeBuilding(BuildingType.Armoury);
            player.UpgradeBuilding(BuildingType.Forge);
            player.UpgradeBuilding(BuildingType.Forge);
            player.UpgradeBuilding(BuildingType.ArcheryRange);
            player.UpgradeBuilding(BuildingType.ArcheryRange);
            player.UpgradeBuilding(BuildingType.ArcheryRange);
            player.TrainTroops(2, 5, 3, 6, 4, 7);

            var result = player.GetArcherInfo();

            result.GetTotalAttack().Should().Be(7 * (int)(50 * 1.2m * 1.3m));
            result.GetTotalDefense().Should().Be(7 * (int)(30 * 1.1m * 1.3m));
        }

        [TestMethod]
        public void Player_GetCavalryInfo_Succeeds() {
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(1000000, 100000, 100000, 100000, 100000));

            player.UpgradeBuilding(BuildingType.Armoury);
            player.UpgradeBuilding(BuildingType.Forge);
            player.UpgradeBuilding(BuildingType.Forge);
            player.UpgradeBuilding(BuildingType.CavalryRange);
            player.UpgradeBuilding(BuildingType.CavalryRange);
            player.UpgradeBuilding(BuildingType.CavalryRange);
            player.TrainTroops(2, 5, 3, 6, 4, 7);

            var result = player.GetCavalryInfo();

            result.GetTotalAttack().Should().Be(9 * (int)(45 * 1.2m * 1.3m));
            result.GetTotalDefense().Should().Be(9 * (int)(35 * 1.1m * 1.3m));
        }

        [TestMethod]
        public void Player_GetFootmanInfo_Succeeds() {
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(1000000, 100000, 100000, 100000, 100000));

            player.UpgradeBuilding(BuildingType.Armoury);
            player.UpgradeBuilding(BuildingType.Forge);
            player.UpgradeBuilding(BuildingType.Forge);
            player.UpgradeBuilding(BuildingType.FootmanRange);
            player.UpgradeBuilding(BuildingType.FootmanRange);
            player.UpgradeBuilding(BuildingType.FootmanRange);
            player.TrainTroops(2, 5, 3, 6, 4, 7);

            var result = player.GetFootmanInfo();

            result.GetTotalAttack().Should().Be(11 * (int)(40 * 1.2m * 1.3m));
            result.GetTotalDefense().Should().Be(11 * (int)(40 * 1.1m * 1.3m));
        }

        [TestMethod]
        public void Player_ProcessAttackDamage_Reduces_Stamina() {
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            player.TrainTroops(150, 50, 150, 50, 150, 50);

            var damage = player.GetArcherInfo().GetTotalDefense() + player.GetCavalryInfo().GetTotalDefense() + player.GetFootmanInfo().GetTotalDefense();

            player.ProcessAttackDamage(damage * 10);

            player.Stamina.Should().Be(80);
        }

        [TestMethod]
        public void Player_ProcessAttackDamage_Reduces_Troops() {
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            player.TrainTroops(150, 50, 150, 50, 150, 50);

            var damage = player.GetArcherInfo().GetTotalDefense() + player.GetCavalryInfo().GetTotalDefense() + player.GetFootmanInfo().GetTotalDefense();

            player.ProcessAttackDamage(damage * 10);

            player.Archers.Soldiers.Should().Be(150);
            player.Archers.Mercenaries.Should().Be(40);
            player.Cavalry.Soldiers.Should().Be(150);
            player.Cavalry.Mercenaries.Should().Be(40);
            player.Footmen.Soldiers.Should().Be(150);
            player.Footmen.Mercenaries.Should().Be(40);
        }

        [TestMethod]
        public void Player_ProcessAttackDamage_Returns_Casualties() {
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(10000000, 1000000, 1000000, 1000000, 1000000));

            player.TrainTroops(150, 50, 150, 50, 150, 50);

            var damage = player.GetArcherInfo().GetTotalDefense() + player.GetCavalryInfo().GetTotalDefense() + player.GetFootmanInfo().GetTotalDefense();

            var casualties = player.ProcessAttackDamage(damage * 10);

            casualties.Archers.Soldiers.Should().Be(0);
            casualties.Archers.Mercenaries.Should().Be(10);
            casualties.Cavalry.Soldiers.Should().Be(0);
            casualties.Cavalry.Mercenaries.Should().Be(10);
            casualties.Footmen.Soldiers.Should().Be(0);
            casualties.Footmen.Mercenaries.Should().Be(10);
        }

        [TestMethod]
        public void Player_ProcessAttack_Succeeds() {
            var attacker = new Player(0, "Test 1");
            var defender = new Player(1, "Test 2");
            var resources = new Resources(20000);

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(attacker, new Resources(100000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(100000));
            typeof(Player).GetProperty(nameof(Player.AttackTurns)).SetValue(attacker, 25);

            attacker.ProcessAttack(defender, resources, 10);

            attacker.Resources.Should().Be(new Resources(120000));
            defender.Resources.Should().Be(new Resources(80000));
            attacker.AttackTurns.Should().Be(15);
        }

        [TestMethod]
        public void Player_CheckUpkeep_Resets_HasUpkeepRunOut_When_Enough() {
            var player = new Player(0, "Test");
            var defender = new Player(1, "Test 2");

            typeof(Player).GetProperty(nameof(Player.HasUpkeepRunOut)).SetValue(player, true);
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(gold: 100000, food: 10000));

            player.ProcessAttack(defender, player.GetUpkeepPerTurn(), 1);
            player.HasUpkeepRunOut.Should().BeFalse();
        }

        [TestMethod]
        public void Player_CheckUpkeepDoes_Not_Reset_HasUpkeepRunOut_When_Not_Enough() {
            var player = new Player(0, "Test");
            var defender = new Player(1, "Test 2");

            typeof(Player).GetProperty(nameof(Player.HasUpkeepRunOut)).SetValue(player, true);
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources());
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(defender, new Resources(gold: 100000, food: 10000));

            player.ProcessAttack(defender, player.GetUpkeepPerTurn() - new Resources(food: 1), 1);
            player.HasUpkeepRunOut.Should().BeTrue();
        }

        [TestMethod]
        public void Player_GetBankCapacity_Succeeds_For_No_Bank_Buildings() {
            var player = new Player(0, "Test");

            player.GetBankCapacity().Should().Be(new Resources());
        }

        [TestMethod]
        public void Player_GetBankCapacity_Succeeds_For_Available_Bank_Buildings() {
            var player = new Player(0, "Test");

            player.Buildings.Add(new Building(player, BuildingType.GoldBank, 1));
            player.Buildings.Add(new Building(player, BuildingType.FoodBank, 2));
            player.Buildings.Add(new Building(player, BuildingType.WoodBank, 3));
            player.Buildings.Add(new Building(player, BuildingType.StoneBank, 4));
            player.Buildings.Add(new Building(player, BuildingType.OreBank, 5));

            player.GetBankCapacity().Should().Be(new Resources(50000, 30000, 50000, 80000, 120000));
        }

        [TestMethod]
        public void Player_GetAvailableBankCapacity_Succeeds() {
            var player = new Player(0, "Test");

            typeof(Player).GetProperty(nameof(Player.BankedResources)).SetValue(player, new Resources(5000, 4000, 3000, 2000, 1000));
            player.Buildings.Add(new Building(player, BuildingType.GoldBank, 1));
            player.Buildings.Add(new Building(player, BuildingType.FoodBank, 2));
            player.Buildings.Add(new Building(player, BuildingType.WoodBank, 3));
            player.Buildings.Add(new Building(player, BuildingType.StoneBank, 4));
            player.Buildings.Add(new Building(player, BuildingType.OreBank, 5));

            player.GetAvailableBankCapacity().Should().Be(new Resources(45000, 26000, 47000, 78000, 119000));
        }

        [TestMethod]
        public void Player_GetBankableResources_Succeeds_With_More_Resources_Than_Capacity() {
            var player = new Player(0, "Test");

            typeof(Player).GetProperty(nameof(Player.BankedResources)).SetValue(player, new Resources(5000, 4000, 3000, 2000, 1000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(60000, 50000, 40000, 30000, 20000));
            player.Buildings.Add(new Building(player, BuildingType.GoldBank, 1));
            player.Buildings.Add(new Building(player, BuildingType.FoodBank, 1));
            player.Buildings.Add(new Building(player, BuildingType.WoodBank, 1));
            player.Buildings.Add(new Building(player, BuildingType.StoneBank, 1));
            player.Buildings.Add(new Building(player, BuildingType.OreBank, 1));

            player.GetBankableResources().Should().Be(new Resources(45000, 16000, 17000, 18000, 19000));
        }

        [TestMethod]
        public void Player_GetBankableResources_Succeeds_With_More_Capacity_Than_Resources() {
            var player = new Player(0, "Test");

            typeof(Player).GetProperty(nameof(Player.BankedResources)).SetValue(player, new Resources(5000, 4000, 3000, 2000, 1000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(6000, 5000, 4000, 3000, 2000));
            player.Buildings.Add(new Building(player, BuildingType.GoldBank, 1));
            player.Buildings.Add(new Building(player, BuildingType.FoodBank, 1));
            player.Buildings.Add(new Building(player, BuildingType.WoodBank, 1));
            player.Buildings.Add(new Building(player, BuildingType.StoneBank, 1));
            player.Buildings.Add(new Building(player, BuildingType.OreBank, 1));

            player.GetBankableResources().Should().Be(new Resources(6000, 5000, 4000, 3000, 2000));
        }
    }
}