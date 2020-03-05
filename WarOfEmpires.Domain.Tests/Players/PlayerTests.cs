using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Siege;

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

            player.Buildings.Add(new Building(BuildingType.Farm, 8));
            player.Buildings.Add(new Building(BuildingType.Lumberyard, 8));
            player.Buildings.Add(new Building(BuildingType.Quarry, 7));
            player.Buildings.Add(new Building(BuildingType.Mine, 7));

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(100000, 10000, 10000, 10000, 10000));
            player.TrainTroops(TroopType.Archers, 2, 0);
            player.TrainTroops(TroopType.Cavalry, 2, 0);
            player.TrainTroops(TroopType.Footmen, 2, 0);

            var previousRecruitingEffort = player.CurrentRecruitingEffort;
            var previousPeasants = player.Peasants;

            while (player.Peasants == previousPeasants) {
                player.Recruit();
            }

            player.CurrentRecruitingEffort.Should().Be(previousRecruitingEffort + 3 % 24);
        }

        [TestMethod]
        public void Player_TrainWorkers_Trains_Workers() {
            var player = new Player(0, "Test");
            var previousStoneMasons = player.GetWorkerCount(WorkerType.StoneMasons);

            player.TrainWorkers(WorkerType.StoneMasons, 4);

            player.GetWorkerCount(WorkerType.StoneMasons).Should().Be(previousStoneMasons + 4);
        }

        [TestMethod]
        public void Player_TrainWorkers_Removes_Peasants() {
            var player = new Player(0, "Test");
            var previousPeasants = player.Peasants;

            player.TrainWorkers(WorkerType.StoneMasons, 4);

            player.Peasants.Should().Be(previousPeasants - 4);
        }

        [TestMethod]
        public void Player_TrainWorkers_Removes_Gold() {
            var player = new Player(0, "Test");
            var previousResources = player.Resources;

            player.TrainWorkers(WorkerType.StoneMasons, 4);

            player.Resources.Should().Be(previousResources - 4 * WorkerDefinitionFactory.Get(WorkerType.StoneMasons).Cost);
        }

        [TestMethod]
        public void Player_UntrainWorkers_Untrains_Workers() {
            var player = new Player(0, "Test");
            player.Workers.Add(new Workers(WorkerType.SiegeEngineers, 5));

            player.UntrainWorkers(WorkerType.SiegeEngineers, 3);

            player.GetWorkerCount(WorkerType.SiegeEngineers).Should().Be(2);
        }

        [TestMethod]
        public void Player_UntrainWorkers_Adds_Peasants() {
            var player = new Player(0, "Test");
            var previousPeasants = player.Peasants;
            player.Workers.Add(new Workers(WorkerType.SiegeEngineers, 5));

            player.UntrainWorkers(WorkerType.SiegeEngineers, 3);

            player.Peasants.Should().Be(previousPeasants + 3);
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

            player.Workers.Add(new Workers(WorkerType.Farmers, 1));
            player.Workers.Add(new Workers(WorkerType.WoodWorkers, 1));
            player.Workers.Add(new Workers(WorkerType.StoneMasons, 2));
            player.Workers.Add(new Workers(WorkerType.OreMiners, 2));
            player.Workers.Add(new Workers(WorkerType.SiegeEngineers, 1));

            player.GetGoldPerTurn().Should().Be(900);
        }

        [DataTestMethod]
        public void Player_GetProduction_Is_Correct() {
            var player = new Player(0, "Test") {
                Tax = 40
            };

            player.Buildings.Add(new Building(BuildingType.Lumberyard, 6));
            player.Workers.Add(new Workers(WorkerType.WoodWorkers, 2));

            player.GetProduction(WorkerType.WoodWorkers).GetTotalProduction().Should().Be(60);
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
            player.Workers.Add(new Workers(WorkerType.Farmers, 1));
            player.Workers.Add(new Workers(WorkerType.WoodWorkers, 2));
            player.Workers.Add(new Workers(WorkerType.StoneMasons, 1));
            player.Workers.Add(new Workers(WorkerType.OreMiners, 2));
            player.Troops.Add(new Troops(TroopType.Archers, 0, 1));

            var previousResources = player.Resources;

            player.Tax = 80;
            player.ProcessTurn();

            player.Resources.Should().Be(previousResources + new Resources(
                player.GetGoldPerTurn(),
                player.GetProduction(WorkerType.Farmers).GetTotalProduction(),
                player.GetProduction(WorkerType.WoodWorkers).GetTotalProduction(),
                player.GetProduction(WorkerType.StoneMasons).GetTotalProduction(),
                player.GetProduction(WorkerType.OreMiners).GetTotalProduction()
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
            player.Workers.Add(new Workers(WorkerType.Farmers, 1));
            player.Workers.Add(new Workers(WorkerType.WoodWorkers, 2));
            player.Workers.Add(new Workers(WorkerType.StoneMasons, 3));
            player.Workers.Add(new Workers(WorkerType.OreMiners, 4));
            player.Tax = 85;

            while (player.CanAfford(player.GetUpkeepPerTurn())) {
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

            while (player.CanAfford(player.GetUpkeepPerTurn())) {
                player.ProcessTurn();
            }

            var previousAttackTurns = player.AttackTurns;

            player.ProcessTurn();

            player.AttackTurns.Should().Be(previousAttackTurns + 1);
        }

        [TestMethod]
        public void Player_ProcessTurn_Increases_Stamina_When_Out_Of_Food_Or_Gold() {
            var player = new Player(0, "Test");

            while (player.CanAfford(player.GetUpkeepPerTurn())) {
                player.ProcessTurn();
            }

            typeof(Player).GetProperty(nameof(Player.Stamina)).SetValue(player, 66);

            player.ProcessTurn();

            player.Stamina.Should().Be(67);
        }

        [TestMethod]
        public void Player_ProcessTurn_Disbands_Mercenaries_When_Out_Of_Food_Or_Gold() {
            var player = new Player(0, "Test");
            player.Tax = 85;

            while (player.Resources.CanAfford(player.GetUpkeepPerTurn())) {
                player.ProcessTurn();
            }

            player.Troops.Add(new Troops(TroopType.Archers, 1, 1));
            player.Troops.Add(new Troops(TroopType.Cavalry, 1, 1));
            player.Troops.Add(new Troops(TroopType.Footmen, 1, 1));

            player.ProcessTurn();

            player.Troops.Should().NotContain(t => t.Mercenaries > 0);
        }

        [TestMethod]
        public void Player_UpgradeBuilding_Succeeds_For_New_BuildingType() {
            var player = new Player(0, "Test");
            var buildingDefinition = BuildingDefinitionFactory.Get(BuildingType.Farm);
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(100000, 10000, 10000, 10000, 10000));

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

            player.Buildings.Clear();
            player.Buildings.Add(new Building(BuildingType.Farm, 1));

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

            player.Buildings.Add(new Building(BuildingType.Farm, 3));

            player.GetBuildingBonusMultiplier(BuildingType.Farm).Should().Be(1.75m);
        }

        [TestMethod]
        public void Player_GetSiegeWeaponCount_Succeeds_For_Nonexistent_SiegeWeaponType() {
            var player = new Player(0, "Test");

            player.GetSiegeWeaponCount(SiegeWeaponType.ScalingLadders).Should().Be(0);
        }

        [TestMethod]
        public void Player_GetSiegeWeaponCount_Succeeds_For_Existing_SiegeWeaponType() {
            var player = new Player(0, "Test");

            player.SiegeWeapons.Add(new SiegeWeapon(SiegeWeaponType.BatteringRams, 5));

            player.GetSiegeWeaponCount(SiegeWeaponType.BatteringRams).Should().Be(5);
        }

        [TestMethod]
        public void Player_GetUpkeepPerTurn_Succeeds() {
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.Peasants)).SetValue(player, 50);
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(100000, 10000, 10000, 10000, 10000));
            player.TrainTroops(TroopType.Archers, 1, 1);
            player.TrainTroops(TroopType.Cavalry, 1, 1);
            player.TrainTroops(TroopType.Footmen, 1, 1);
            player.TrainWorkers(WorkerType.Farmers, 1);
            player.TrainWorkers(WorkerType.WoodWorkers, 1);
            player.TrainWorkers(WorkerType.StoneMasons, 1);
            player.TrainWorkers(WorkerType.OreMiners, 1);
            player.TrainWorkers(WorkerType.SiegeEngineers, 1);

            player.GetUpkeepPerTurn().Should().Be(new Resources(food: 106, gold: 750));
        }

        [TestMethod]
        public void Player_GetTheoreticalRecruitsPerDay_Succeeds() {
            var player = new Player(0, "Test");

            player.Buildings.Add(new Building(BuildingType.Farm, 4));
            player.Buildings.Add(new Building(BuildingType.Lumberyard, 6));
            player.Buildings.Add(new Building(BuildingType.Quarry, 2));
            player.Buildings.Add(new Building(BuildingType.Mine, 2));
            player.Buildings.Add(new Building(BuildingType.Defences, 3));

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(100000, 10000, 10000, 10000, 10000));
            player.TrainTroops(TroopType.Archers, 2, 0);
            player.TrainTroops(TroopType.Cavalry, 2, 0);
            player.TrainTroops(TroopType.Footmen, 2, 0);

            player.GetTheoreticalRecruitsPerDay().Should().Be(8);
        }

        [TestMethod]
        public void Player_GetTheoreticalRecruitsPerDay_Uses_Soldier_Penalty() {
            var player = new Player(0, "Test");

            player.Buildings.Add(new Building(BuildingType.Farm, 4));
            player.Buildings.Add(new Building(BuildingType.Lumberyard, 6));
            player.Buildings.Add(new Building(BuildingType.Quarry, 2));
            player.Buildings.Add(new Building(BuildingType.Mine, 2));
            player.Buildings.Add(new Building(BuildingType.Defences, 3));

            player.GetTheoreticalRecruitsPerDay().Should().Be(5);
        }

        [TestMethod]
        public void Player_GetTheoreticalRecruitsPerDay_Minimum_Is_1() {
            var player = new Player(0, "Test");

            player.GetTheoreticalRecruitsPerDay().Should().Be(1);
        }

        [TestMethod]
        public void Player_GetTheoreticalRecruitsPerDay_Maximum_Is_25() {
            var player = new Player(0, "Test");

            player.Buildings.Add(new Building(BuildingType.Farm, 15));
            player.Buildings.Add(new Building(BuildingType.Lumberyard, 15));
            player.Buildings.Add(new Building(BuildingType.Quarry, 15));
            player.Buildings.Add(new Building(BuildingType.Mine, 15));
            player.Buildings.Add(new Building(BuildingType.Defences, 15));

            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(100000, 10000, 10000, 10000, 10000));
            player.TrainTroops(TroopType.Archers, 2, 0);
            player.TrainTroops(TroopType.Cavalry, 2, 0);
            player.TrainTroops(TroopType.Footmen, 2, 0);

            player.GetTheoreticalRecruitsPerDay().Should().Be(25);
        }

        [TestMethod]
        public void Player_GetAvailableHousingCapacity_Succeeds() {
            var player = new Player(0, "Test");

            player.Troops.Add(new Troops(TroopType.Archers, 19, 0));
            typeof(Player).GetProperty(nameof(Player.Peasants)).SetValue(player, 10);

            player.GetAvailableHousingCapacity().Should().Be(11);
        }

        [TestMethod]
        public void Player_GetAvailableHousingCapacity_Succeeds_For_Crowded_Barracks() {
            var player = new Player(0, "Test");

            player.Troops.Add(new Troops(TroopType.Archers, 6, 1));
            player.Troops.Add(new Troops(TroopType.Cavalry, 6, 1));
            player.Troops.Add(new Troops(TroopType.Footmen, 6, 1));
            typeof(Player).GetProperty(nameof(Player.Peasants)).SetValue(player, 10);

            player.GetAvailableHousingCapacity().Should().Be(9);
        }

        [TestMethod]
        public void Player_GetAvailableHousingCapacity_Succeeds_For_Crowded_Huts() {
            var player = new Player(0, "Test");

            typeof(Player).GetProperty(nameof(Player.Peasants)).SetValue(player, 5);
            player.Workers.Add(new Workers(WorkerType.Farmers, 4));
            player.Workers.Add(new Workers(WorkerType.WoodWorkers, 4));
            player.Workers.Add(new Workers(WorkerType.StoneMasons, 4));
            player.Workers.Add(new Workers(WorkerType.OreMiners, 4));
            player.Workers.Add(new Workers(WorkerType.SiegeEngineers, 3));

            player.GetAvailableHousingCapacity().Should().Be(16);
        }

        [TestMethod]
        public void Player_GetRecruitsPerDay_Is_Maxed_By_Barracks_And_Huts_Capacity() {
            var player = new Player(0, "Test");

            player.Buildings.Add(new Building(BuildingType.Farm, 4));
            player.Buildings.Add(new Building(BuildingType.Lumberyard, 6));
            player.Buildings.Add(new Building(BuildingType.Quarry, 2));
            player.Buildings.Add(new Building(BuildingType.Mine, 2));
            player.Buildings.Add(new Building(BuildingType.Defences, 3));

            player.Troops.Add(new Troops(TroopType.Archers, 19, 0));
            typeof(Player).GetProperty(nameof(Player.Peasants)).SetValue(player, 20);

            player.GetRecruitsPerDay().Should().Be(1);
        }

        [TestMethod]
        public void Player_GetTotalGoldSpentOnBuildings_Succeeds() {
            var player = new Player(0, "Test");

            player.Buildings.Add(new Building(BuildingType.Farm, 4));
            player.Buildings.Add(new Building(BuildingType.Lumberyard, 8));
            player.Buildings.Add(new Building(BuildingType.Quarry, 2));
            player.Buildings.Add(new Building(BuildingType.Mine, 2));

            player.GetTotalGoldSpentOnBuildings().Should().Be(1580000);
        }

        [TestMethod]
        public void Player_GetTotalGoldSpentOnBuildings_Ignores_Defences() {
            var player = new Player(0, "Test");

            player.Buildings.Add(new Building(BuildingType.Defences, 10));

            player.GetTotalGoldSpentOnBuildings().Should().Be(0);
        }

        [TestMethod]
        public void Player_TrainTroops_Trains_Troops_Existing_TroopType() {
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(200000, 10000, 10000, 10000, 10000));

            player.Troops.Add(new Troops(TroopType.Cavalry, 1, 1));

            player.TrainTroops(TroopType.Cavalry, 1, 4);

            player.Troops.Should().HaveCount(1);
            player.Troops.Single().Soldiers.Should().Be(2);
            player.Troops.Single().Mercenaries.Should().Be(5);
        }

        [TestMethod]
        public void Player_TrainTroops_Trains_Troops_New_TroopType() {
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(200000, 10000, 10000, 10000, 10000));

            player.Troops.Add(new Troops(TroopType.Archers, 1, 1));

            player.TrainTroops(TroopType.Cavalry, 1, 4);

            player.Troops.Should().HaveCount(2);
            player.Troops.Single(t => t.Type == TroopType.Cavalry).Soldiers.Should().Be(1);
            player.Troops.Single(t => t.Type == TroopType.Cavalry).Mercenaries.Should().Be(4);
        }

        [TestMethod]
        public void Player_TrainTroops_Removes_Peasants() {
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(200000, 10000, 10000, 10000, 10000));

            player.TrainTroops(TroopType.Cavalry, 1, 4);

            player.Peasants.Should().Be(9);
        }

        [TestMethod]
        public void Player_TrainTroops_Costs_Resources() {
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(200000, 10000, 10000, 10000, 10000));

            player.TrainTroops(TroopType.Archers, 1, 4);
            player.TrainTroops(TroopType.Cavalry, 2, 5);
            player.TrainTroops(TroopType.Footmen, 3, 6);

            player.Resources.Should().Be(new Resources(95000, 10000, 7500, 10000, 3500));
        }

        [TestMethod]
        public void Player_UntrainTroops_Removes_Troops() {
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(200000, 10000, 10000, 10000, 10000));
            player.TrainTroops(TroopType.Archers, 2, 5);

            player.UntrainTroops(TroopType.Archers, 1, 4);
            
            player.Troops.Single(t => t.Type == TroopType.Archers).Soldiers.Should().Be(1);
            player.Troops.Single(t => t.Type == TroopType.Archers).Mercenaries.Should().Be(1);
        }

        [TestMethod]
        public void Player_UntrainTroops_Adds_Peasants() {
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(200000, 10000, 10000, 10000, 10000));
            player.TrainTroops(TroopType.Archers, 2, 5);

            player.UntrainTroops(TroopType.Archers, 1, 4);

            player.Peasants.Should().Be(9);
        }

        [DataTestMethod]
        [DataRow(TroopType.Archers, BuildingType.ArcheryRange, 11 * (int)(50 * 1.2m * 1.3m), 11 * (int)(30 * 1.1m * 1.3m), DisplayName = "Archers")]
        [DataRow(TroopType.Cavalry, BuildingType.CavalryRange, 11 * (int)(45 * 1.2m * 1.3m), 11 * (int)(35 * 1.1m * 1.3m), DisplayName = "Cavalry")]
        [DataRow(TroopType.Footmen, BuildingType.FootmanRange, 11 * (int)(40 * 1.2m * 1.3m), 11 * (int)(40 * 1.1m * 1.3m), DisplayName = "Footmen")]
        public void Player_GetTroopInfo_Succeeds(TroopType type, BuildingType buildingType, int attack, int defence) {
            var player = new Player(0, "Test");

            player.Buildings.Add(new Building(BuildingType.Armoury, 1));
            player.Buildings.Add(new Building(BuildingType.Forge, 2));
            player.Buildings.Add(new Building(buildingType, 3));
            player.Troops.Add(new Troops(type, 4, 7));

            var result = player.GetTroopInfo(type);

            result.GetTotalAttack().Should().Be(attack);
            result.GetTotalDefense().Should().Be(defence);
        }

        [TestMethod]
        public void Player_ProcessAttackDamage_Reduces_Stamina() {
            var player = new Player(0, "Test");

            player.Troops.Add(new Troops(TroopType.Archers, 150, 50));
            player.Troops.Add(new Troops(TroopType.Cavalry, 150, 50));
            player.Troops.Add(new Troops(TroopType.Footmen, 150, 50));

            var damage = player.Troops.Sum(t => player.GetTroopInfo(t.Type).GetTotalDefense());

            player.ProcessAttackDamage(damage * 10);

            player.Stamina.Should().Be(80);
        }

        [TestMethod]
        public void Player_ProcessAttackDamage_Reduces_Troops() {
            var player = new Player(0, "Test");

            player.Troops.Add(new Troops(TroopType.Archers, 150, 50));
            player.Troops.Add(new Troops(TroopType.Cavalry, 150, 50));
            player.Troops.Add(new Troops(TroopType.Footmen, 150, 50));

            var damage = player.Troops.Sum(t => player.GetTroopInfo(t.Type).GetTotalDefense());

            player.ProcessAttackDamage(damage * 10);

            foreach (var troops in player.Troops) {
                troops.Soldiers.Should().Be(150);
                troops.Mercenaries.Should().Be(40);
            }
        }

        [TestMethod]
        public void Player_ProcessAttackDamage_Returns_Casualties() {
            var player = new Player(0, "Test");

            player.Troops.Add(new Troops(TroopType.Archers, 150, 50));
            player.Troops.Add(new Troops(TroopType.Cavalry, 150, 50));
            player.Troops.Add(new Troops(TroopType.Footmen, 150, 50));

            var damage = player.Troops.Sum(t => player.GetTroopInfo(t.Type).GetTotalDefense());

            var casualties = player.ProcessAttackDamage(damage * 10);

            casualties.Should().HaveCount(3);

            foreach (var c in casualties) {
                c.Soldiers.Should().Be(0);
                c.Mercenaries.Should().Be(10);
            }
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

            player.Buildings.Add(new Building(BuildingType.GoldBank, 1));
            player.Buildings.Add(new Building(BuildingType.FoodBank, 2));
            player.Buildings.Add(new Building(BuildingType.WoodBank, 3));
            player.Buildings.Add(new Building(BuildingType.StoneBank, 4));
            player.Buildings.Add(new Building(BuildingType.OreBank, 5));

            player.GetBankCapacity().Should().Be(new Resources(50000, 30000, 50000, 80000, 120000));
        }

        [TestMethod]
        public void Player_GetAvailableBankCapacity_Succeeds() {
            var player = new Player(0, "Test");

            typeof(Player).GetProperty(nameof(Player.BankedResources)).SetValue(player, new Resources(5000, 4000, 3000, 2000, 1000));
            player.Buildings.Add(new Building(BuildingType.GoldBank, 1));
            player.Buildings.Add(new Building(BuildingType.FoodBank, 2));
            player.Buildings.Add(new Building(BuildingType.WoodBank, 3));
            player.Buildings.Add(new Building(BuildingType.StoneBank, 4));
            player.Buildings.Add(new Building(BuildingType.OreBank, 5));

            player.GetAvailableBankCapacity().Should().Be(new Resources(45000, 26000, 47000, 78000, 119000));
        }

        [TestMethod]
        public void Player_GetBankableResources_Succeeds_With_More_Resources_Than_Capacity() {
            var player = new Player(0, "Test");

            typeof(Player).GetProperty(nameof(Player.BankedResources)).SetValue(player, new Resources(5000, 4000, 3000, 2000, 1000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(60000, 50000, 40000, 30000, 20000));
            player.Buildings.Add(new Building(BuildingType.GoldBank, 1));
            player.Buildings.Add(new Building(BuildingType.FoodBank, 1));
            player.Buildings.Add(new Building(BuildingType.WoodBank, 1));
            player.Buildings.Add(new Building(BuildingType.StoneBank, 1));
            player.Buildings.Add(new Building(BuildingType.OreBank, 1));

            player.GetBankableResources().Should().Be(new Resources(45000, 16000, 17000, 18000, 19000));
        }

        [TestMethod]
        public void Player_GetBankableResources_Succeeds_With_More_Capacity_Than_Resources() {
            var player = new Player(0, "Test");

            typeof(Player).GetProperty(nameof(Player.BankedResources)).SetValue(player, new Resources(5000, 4000, 3000, 2000, 1000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(6000, 5000, 4000, 3000, 2000));
            player.Buildings.Add(new Building(BuildingType.GoldBank, 1));
            player.Buildings.Add(new Building(BuildingType.FoodBank, 1));
            player.Buildings.Add(new Building(BuildingType.WoodBank, 1));
            player.Buildings.Add(new Building(BuildingType.StoneBank, 1));
            player.Buildings.Add(new Building(BuildingType.OreBank, 1));

            player.GetBankableResources().Should().Be(new Resources(6000, 5000, 4000, 3000, 2000));
        }

        [TestMethod]
        public void Player_AddBankTurn_Adds_BankTurn() {
            var player = new Player(0, "Test");
            var previousBankTurns = player.BankTurns;

            player.AddBankTurn();

            player.BankTurns.Should().Be(previousBankTurns + 1);
        }

        [TestMethod]
        public void Player_Bank_Succeeds() {
            var player = new Player(0, "Test");
            var previousBankTurns = player.BankTurns;

            typeof(Player).GetProperty(nameof(Player.BankedResources)).SetValue(player, new Resources(25000, 10000, 10000, 10000, 10000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(35000, 5000, 15000, 5000, 15000));
            player.Buildings.Add(new Building(BuildingType.GoldBank, 1));
            player.Buildings.Add(new Building(BuildingType.FoodBank, 1));
            player.Buildings.Add(new Building(BuildingType.WoodBank, 1));
            player.Buildings.Add(new Building(BuildingType.StoneBank, 1));
            player.Buildings.Add(new Building(BuildingType.OreBank, 1));

            player.Bank();

            player.BankTurns.Should().Be(previousBankTurns - 1);
            player.Resources.Should().Be(new Resources(10000, 0, 5000, 0, 5000));
            player.BankedResources.Should().Be(new Resources(50000, 15000, 20000, 15000, 20000));
        }

        [TestMethod]
        public void Player_SpendResources_Spends_Resources_First() {
            var player = new Player(0, "Test");

            typeof(Player).GetProperty(nameof(Player.BankedResources)).SetValue(player, new Resources(10000, 10000, 10000, 10000, 10000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(10000, 10000, 10000, 10000, 10000));

            player.SpendResources(new Resources(10000, 5000, 5000, 5000, 5000));
            player.Resources.Should().Be(new Resources(0, 5000, 5000, 5000, 5000));
            player.BankedResources.Should().Be(new Resources(10000, 10000, 10000, 10000, 10000));
        }

        [TestMethod]
        public void Player_SpendResources_Spends_BankedResources_Second() {
            var player = new Player(0, "Test");

            typeof(Player).GetProperty(nameof(Player.BankedResources)).SetValue(player, new Resources(10000, 10000, 10000, 10000, 10000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(10000, 10000, 10000, 10000, 10000));

            player.SpendResources(new Resources(20000, 15000, 15000, 15000, 15000));
            player.Resources.Should().Be(new Resources());
            player.BankedResources.Should().Be(new Resources(0, 5000, 5000, 5000, 5000));
        }

        [TestMethod]
        public void Player_CanAfford_Succeeds_When_Enough() {
            var player = new Player(0, "Test");

            typeof(Player).GetProperty(nameof(Player.BankedResources)).SetValue(player, new Resources(10000, 10000, 10000, 10000, 10000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(10000, 10000, 10000, 10000, 10000));

            player.CanAfford(new Resources(20000, 20000, 20000, 20000, 20000)).Should().BeTrue();
        }

        [DataTestMethod]
        [DataRow(20001, 0, 0, 0, 0, DisplayName = "Gold")]
        [DataRow(0, 20001, 0, 0, 0, DisplayName = "Food")]
        [DataRow(0, 0, 20001, 0, 0, DisplayName = "Wood")]
        [DataRow(0, 0, 0, 20001, 0, DisplayName = "Stone")]
        [DataRow(0, 0, 0, 0, 20001, DisplayName = "Ore")]
        public void Player_CanAfford_Succeeds_When_Not_Enough(int gold, int food, int wood, int stone, int ore) {
            var player = new Player(0, "Test");

            typeof(Player).GetProperty(nameof(Player.BankedResources)).SetValue(player, new Resources(10000, 10000, 10000, 10000, 10000));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(10000, 10000, 10000, 10000, 10000));

            player.CanAfford(new Resources(gold, food, wood, stone, ore));
        }

        [DataTestMethod]
        [DataRow(20, 0, 0, DisplayName = "All soldiers")]
        [DataRow(10, 10, 0, DisplayName = "Perfectly balanced")]
        [DataRow(9, 11, 1, DisplayName = "Just too few")]
        [DataRow(8, 12, 2, DisplayName = "Too few")]
        [DataRow(7, 13, 3, DisplayName = "Way too few")]
        public void Player_GetSoldierRecruitsPenalty_Succeeds(int soldiers, int peasants, int expectedResult) {
            var player = new Player(0, "Test");

            player.Troops.Add(new Troops(TroopType.Archers, soldiers, 50));
            typeof(Player).GetProperty(nameof(Player.Peasants)).SetValue(player, peasants);

            player.GetSoldierRecruitsPenalty().Should().Be(expectedResult);
        }

        [TestMethod]
        public void Player_BuildSiege_Succeeds_For_New_SiegeWeaponType() {
            var player = new Player(0, "Test");
            var siegeWeaponDefinition = SiegeWeaponDefinitionFactory.Get(SiegeWeaponType.FireArrows);
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(100000, 10000, 10000, 10000, 10000));

            var previousResources = player.Resources;

            player.BuildSiege(SiegeWeaponType.FireArrows, 3);

            player.SiegeWeapons.Single().Count.Should().Be(3);
            player.SiegeWeapons.Single().Type.Should().Be(SiegeWeaponType.FireArrows);
            player.Resources.Should().Be(previousResources - siegeWeaponDefinition.Cost * 3);
        }

        [TestMethod]
        public void Player_BuildSiege_Succeeds_For_Existing_SiegeWeaponType() {
            var player = new Player(0, "Test");
            var siegeWeaponDefinition = SiegeWeaponDefinitionFactory.Get(SiegeWeaponType.FireArrows);
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(100000, 10000, 10000, 10000, 10000));

            var previousResources = player.Resources;

            player.SiegeWeapons.Add(new SiegeWeapon(SiegeWeaponType.FireArrows, 2));
            player.BuildSiege(SiegeWeaponType.FireArrows, 3);

            player.SiegeWeapons.Single().Count.Should().Be(5);
            player.SiegeWeapons.Single().Type.Should().Be(SiegeWeaponType.FireArrows);
            player.Resources.Should().Be(previousResources - siegeWeaponDefinition.Cost * 3);
        }

        [TestMethod]
        public void Player_DiscardSiege_Succeeds() {
            var player = new Player(0, "Test");
            player.SiegeWeapons.Add(new SiegeWeapon(SiegeWeaponType.FireArrows, 5));

            player.DiscardSiege(SiegeWeaponType.FireArrows, 3);

            player.SiegeWeapons.Single().Count.Should().Be(2);
        }

        [TestMethod]
        public void Player_HealTroops_Succeeds() {
            var player = new Player(0, "Test");            
            player.Troops.Add(new Troops(TroopType.Archers, 30, 10));
            player.Troops.Add(new Troops(TroopType.Cavalry, 20, 10));
            player.Troops.Add(new Troops(TroopType.Footmen, 20, 10));
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(1000000, 100000, 100000, 100000, 100000));
            typeof(Player).GetProperty(nameof(Player.Stamina)).SetValue(player, 90);
            var previousResources = player.Resources;
            player.HealTroops(10);
            player.Stamina.Should().Be(100);
            player.Resources.Should().Be(previousResources - new Resources(food: 2000));
        }

        [TestMethod]
        public void Player_HealTroops_Succeeds_For_Zero_Troops() {
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(1000000, 100000, 100000, 100000, 100000));
            typeof(Player).GetProperty(nameof(Player.Stamina)).SetValue(player, 90);
            var previousResources = player.Resources;
            player.HealTroops(10);
            player.Stamina.Should().Be(100);
            player.Resources.Should().Be(previousResources);
        }

        [TestMethod]
        public void Player_GetStaminaToHeal_Correct_For_Can_Afford() {
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(food: 100000));
            typeof(Player).GetProperty(nameof(Player.Stamina)).SetValue(player, 90);
            player.Troops.Add(new Troops(TroopType.Archers, 30, 10));
            player.Troops.Add(new Troops(TroopType.Cavalry, 20, 10));
            player.Troops.Add(new Troops(TroopType.Footmen, 20, 10));
            player.GetStaminaToHeal().Should().Be(10);
        }

        [TestMethod]
        public void Player_GetStaminaToHeal_Correct_For_Cant_Afford() {
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(food: 1000));
            typeof(Player).GetProperty(nameof(Player.Stamina)).SetValue(player, 90);
            player.Troops.Add(new Troops(TroopType.Archers, 30, 10));
            player.Troops.Add(new Troops(TroopType.Cavalry, 20, 10));
            player.Troops.Add(new Troops(TroopType.Footmen, 20, 10));
            player.GetStaminaToHeal().Should().Be(5);
        }

        [TestMethod]
        public void Player_GetStaminaToHeal_Correct_For_Zero_Troops_Zero_Food() {
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(food: 100000));
            typeof(Player).GetProperty(nameof(Player.Stamina)).SetValue(player, 90);
            player.Troops.Add(new Troops(TroopType.Archers, 0, 0));
            player.Troops.Add(new Troops(TroopType.Cavalry, 0, 0));
            player.Troops.Add(new Troops(TroopType.Footmen, 0, 0));
            player.GetStaminaToHeal().Should().Be(10);
        }

        [TestMethod]
        public void Player_GetStaminaToHeal_Correct_For_Zero_Food() {
            var player = new Player(0, "Test");
            typeof(Player).GetProperty(nameof(Player.Resources)).SetValue(player, new Resources(food: 0));
            typeof(Player).GetProperty(nameof(Player.Stamina)).SetValue(player, 90);
            player.Troops.Add(new Troops(TroopType.Archers, 30, 10));
            player.Troops.Add(new Troops(TroopType.Cavalry, 20, 10));
            player.Troops.Add(new Troops(TroopType.Footmen, 20, 10));
            player.GetStaminaToHeal().Should().Be(0);
        }

        [DataTestMethod]
        [DataRow(TroopType.Archers, 10, 0, 0, DisplayName = "Archers, no weapons")]
        [DataRow(TroopType.Archers, 90, 2, 72, DisplayName = "Archers, insufficient weapons")]
        [DataRow(TroopType.Archers, 72, 2, 72, DisplayName = "Archers, exactly enough weapons")]
        [DataRow(TroopType.Archers, 60, 2, 60, DisplayName = "Archers, too many weapons")]
        [DataRow(TroopType.Cavalry, 60, 2, 16, DisplayName = "Cavalry, insufficient weapons")]
        [DataRow(TroopType.Footmen, 60, 2, 24, DisplayName = "Footmen, insufficient weapons")]
        public void Player_GetSiegeWeaponTroopCount_Succeeds(TroopType troopType, int troopCount, int siegeWeaponCount, int expectedResult) {
            var player = new Player(0, "Test");

            player.Troops.Add(new Troops(troopType, troopCount, 0));
            player.SiegeWeapons.Add(new SiegeWeapon(SiegeWeaponDefinitionFactory.Get(troopType).Type, siegeWeaponCount));
            player.GetSiegeWeaponTroopCount(troopType).Should().Be(expectedResult);
        }

        [TestMethod]
        public void Player_GetTroops_Succeeds_For_Nonexistent_TroopType() {
            var player = new Player(0, "Test");

            var troops = player.GetTroops(TroopType.Archers);

            troops.Type.Should().Be(TroopType.Archers);
            troops.Soldiers.Should().Be(0);
            troops.Mercenaries.Should().Be(0);
        }

        [TestMethod]
        public void Player_GetTroops_Succeeds_For_Existing_TroopType() {
            var player = new Player(0, "Test");

            player.Troops.Add(new Troops(TroopType.Cavalry, 10, 2));

            var troops = player.GetTroops(TroopType.Cavalry);

            troops.Type.Should().Be(TroopType.Cavalry);
            troops.Soldiers.Should().Be(10);
            troops.Mercenaries.Should().Be(2);
        }

        [TestMethod]
        public void Player_GetWorkers_Succeeds_For_Nonexistent_WorkerType() {
            var player = new Player(0, "Test");

            player.GetWorkerCount(WorkerType.StoneMasons).Should().Be(0);
        }

        [TestMethod]
        public void Player_GetWorkers_Succeeds_For_Existing_WorkerType() {
            var player = new Player(0, "Test");

            player.Workers.Add(new Workers(WorkerType.OreMiners, 7));

            player.GetWorkerCount(WorkerType.OreMiners).Should().Be(7);
        }
    }
}