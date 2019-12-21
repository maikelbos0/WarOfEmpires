using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Empires;

namespace WarOfEmpires.Domain.Players {
    public class Player : AggregateRoot {
        public const int RecruitingEffortStep = 24;
        public const int BaseGoldProduction = 500;
        public const int BaseResourceProduction = 20;
        public static int[] BuildingRecruitingLevels = { 50000, 100000, 200000, 300000, 500000, 800000, 1200000, 2000000, 3000000, 5000000, 8000000, 12000000, 20000000, 30000000, 40000000, 50000000, 60000000, 70000000, 80000000, 90000000, 100000000, 110000000, 120000000, 130000000, 140000000, 150000000 };
        public const long BaseArcherAttack = 50;
        public const long BaseArcherDefense = 30;
        public const long BaseCavalryAttack = 45;
        public const long BaseCavalryDefense = 35;
        public const long BaseFootmanAttack = 40;
        public const long BaseFootmanDefense = 40;
        public const int AttackDamageModifier = 200;
        public const int AttackStaminaDrainModifier = 2;

        public static Resources WorkerTrainingCost = new Resources(gold: 250);
        public static Resources ArcherTrainingCost = new Resources(gold: 5000, wood: 1000, ore: 500);
        public static Resources CavalryTrainingCost = new Resources(gold: 5000, ore: 1500);
        public static Resources FootmanTrainingCost = new Resources(gold: 5000, wood: 500, ore: 1000);
        public static Resources MercenaryTrainingCost = new Resources(gold: 5000);

        public static Resources PeasantUpkeep = new Resources(food: 2);
        public static Resources SoldierUpkeep = new Resources(food: 5);
        public static Resources MercenaryUpkeep = new Resources(gold: 250, food: 5);

        public virtual string DisplayName { get; set; }
        public virtual Security.User User { get; protected set; }
        /// <summary>
        /// Current number of expected recruits / 24
        /// </summary>
        public virtual int CurrentRecruitingEffort { get; protected set; } = 0;
        public virtual int Peasants { get; protected set; } = 10;
        public virtual int Farmers { get; protected set; }
        public virtual int WoodWorkers { get; protected set; }
        public virtual int StoneMasons { get; protected set; }
        public virtual int OreMiners { get; protected set; }
        public virtual Resources Resources { get; protected set; } = new Resources(10000, 2000, 2000, 2000, 2000);
        public virtual int Tax { get; set; } = 50;
        public virtual Troops Archers { get; protected set; } = new Troops(0, 0);
        public virtual Troops Cavalry { get; protected set; } = new Troops(0, 0);
        public virtual Troops Footmen { get; protected set; } = new Troops(0, 0);
        public virtual int AttackTurns { get; protected set; } = 50;
        public virtual int Stamina { get; protected set; } = 100;
        public virtual ICollection<Building> Buildings { get; protected set; } = new List<Building>();
        public virtual ICollection<Message> SentMessages { get; protected set; } = new List<Message>();
        public virtual ICollection<Message> ReceivedMessages { get; protected set; } = new List<Message>();
        public virtual ICollection<Attack> ExecutedAttacks { get; protected set; } = new List<Attack>();
        public virtual ICollection<Attack> ReceivedAttacks { get; protected set; } = new List<Attack>();

        protected Player() {
        }

        public Player(int id, string displayName) {
            Id = id;
            DisplayName = displayName;

            foreach (var building in GetStartingBuildings()) {
                Buildings.Add(building);
            }
        }

        public IEnumerable<Building> GetStartingBuildings() {
            yield return new Building(this, BuildingType.Barracks, 2);
            yield return new Building(this, BuildingType.Huts, 2);
        }

        public decimal GetTaxRate() {
            return (decimal)Tax / 100;
        }

        public int GetBaseGoldPerTurn() {
            return BaseGoldProduction;
        }

        public int GetGoldPerWorkerPerTurn() {
            return (int)(GetTaxRate() * GetBaseGoldPerTurn());
        }

        public int GetGoldPerTurn() {
            return GetGoldPerWorkerPerTurn() * (Farmers + WoodWorkers + StoneMasons + OreMiners);
        }

        public ProductionInfo GetFoodProduction() {
            return new ProductionInfo(Farmers, GetBuildingBonusMultiplier(BuildingType.Farm), GetTaxRate());
        }

        public ProductionInfo GetWoodProduction() {
            return new ProductionInfo(WoodWorkers, GetBuildingBonusMultiplier(BuildingType.Lumberyard), GetTaxRate());
        }

        public ProductionInfo GetStoneProduction() {
            return new ProductionInfo(StoneMasons, GetBuildingBonusMultiplier(BuildingType.Quarry), GetTaxRate());
        }

        public ProductionInfo GetOreProduction() {
            return new ProductionInfo(OreMiners, GetBuildingBonusMultiplier(BuildingType.Mine), GetTaxRate());
        }

        public virtual Resources GetUpkeepPerTurn() {
            var upkeep = new Resources();

            upkeep += (Peasants + Farmers + WoodWorkers + StoneMasons + OreMiners) * PeasantUpkeep;
            upkeep += (Archers.Soldiers + Cavalry.Soldiers + Footmen.Soldiers) * SoldierUpkeep;
            upkeep += (Archers.Mercenaries + Cavalry.Mercenaries + Footmen.Mercenaries) * MercenaryUpkeep;

            return upkeep;
        }

        public virtual TroopInfo GetArcherInfo() {
            return new TroopInfo(Archers, BaseArcherAttack, BaseArcherDefense, GetBuildingBonusMultiplier(BuildingType.ArcheryRange),
                GetBuildingBonusMultiplier(BuildingType.Forge), GetBuildingBonusMultiplier(BuildingType.Armoury));
        }

        public virtual TroopInfo GetCavalryInfo() {
            return new TroopInfo(Cavalry, BaseCavalryAttack, BaseCavalryDefense, GetBuildingBonusMultiplier(BuildingType.CavalryRange),
                GetBuildingBonusMultiplier(BuildingType.Forge), GetBuildingBonusMultiplier(BuildingType.Armoury));
        }

        public virtual TroopInfo GetFootmanInfo() {
            return new TroopInfo(Footmen, BaseFootmanAttack, BaseFootmanDefense, GetBuildingBonusMultiplier(BuildingType.FootmanRange),
                GetBuildingBonusMultiplier(BuildingType.Forge), GetBuildingBonusMultiplier(BuildingType.Armoury));
        }

        public virtual Casualties ProcessAttackDamage(long damage) {
            var totalDefense = GetArcherInfo().GetTotalDefense() + GetCavalryInfo().GetTotalDefense() + GetFootmanInfo().GetTotalDefense();
            var archerCasualties = Archers.GetTroopCasualties((int)(GetArcherInfo().GetTotalDefense() * damage / totalDefense / AttackDamageModifier / GetArcherInfo().GetDefensePerSoldier()));
            var cavalryCasualties = Cavalry.GetTroopCasualties((int)(GetCavalryInfo().GetTotalDefense() * damage / totalDefense / AttackDamageModifier / GetCavalryInfo().GetDefensePerSoldier()));
            var footmanCasualties = Footmen.GetTroopCasualties((int)(GetFootmanInfo().GetTotalDefense() * damage / totalDefense / AttackDamageModifier / GetFootmanInfo().GetDefensePerSoldier()));

            Stamina = Math.Max(0, (int)(Stamina - damage * AttackStaminaDrainModifier / totalDefense));
            Archers -= archerCasualties;
            Cavalry -= cavalryCasualties;
            Footmen -= footmanCasualties;

            return new Casualties(archerCasualties, cavalryCasualties, footmanCasualties);
        }

        public virtual void Recruit() {
            CurrentRecruitingEffort += GetRecruitsPerDay();

            if (CurrentRecruitingEffort >= RecruitingEffortStep) {
                var newRecruits = CurrentRecruitingEffort / RecruitingEffortStep;

                CurrentRecruitingEffort -= newRecruits * RecruitingEffortStep;
                Peasants += newRecruits;
            }
        }

        public virtual void ProcessTurn() {
            var upkeep = GetUpkeepPerTurn();

            AttackTurns++;

            if (Stamina < 100) {
                Stamina++;
            }

            if (Resources.CanAfford(upkeep)) {
                Resources = Resources - upkeep + new Resources(
                    GetGoldPerTurn(),
                    GetFoodProduction().GetTotalProduction(),
                    GetWoodProduction().GetTotalProduction(),
                    GetStoneProduction().GetTotalProduction(),
                    GetOreProduction().GetTotalProduction()
                );
            }
            else {
                Resources -= new Resources(
                    Math.Min(upkeep.Gold, Resources.Gold),
                    Math.Min(upkeep.Food, Resources.Food),
                    Math.Min(upkeep.Wood, Resources.Wood),
                    Math.Min(upkeep.Stone, Resources.Stone),
                    Math.Min(upkeep.Ore, Resources.Ore)
                );

                Archers = new Troops(Archers.Soldiers, 0);
                Cavalry = new Troops(Cavalry.Soldiers, 0);
                Footmen = new Troops(Footmen.Soldiers, 0);
            }
        }

        public virtual void TrainWorkers(int farmers, int woodWorkers, int stoneMasons, int oreMiners) {
            var trainedPeasants = farmers + woodWorkers + stoneMasons + oreMiners;

            Farmers += farmers;
            WoodWorkers += woodWorkers;
            StoneMasons += stoneMasons;
            OreMiners += oreMiners;

            Peasants -= trainedPeasants;
            Resources -= trainedPeasants * WorkerTrainingCost;
        }

        public virtual void UntrainWorkers(int farmers, int woodWorkers, int stoneMasons, int oreMiners) {
            Farmers -= farmers;
            WoodWorkers -= woodWorkers;
            StoneMasons -= stoneMasons;
            OreMiners -= oreMiners;

            Peasants += farmers + woodWorkers + stoneMasons + oreMiners;
        }

        public virtual void UpgradeBuilding(BuildingType type) {
            var definition = BuildingDefinitionFactory.Get(type);
            var building = Buildings.SingleOrDefault(b => b.Type == type);

            Resources -= definition.GetNextLevelCost(building?.Level ?? 0);

            if (building == null) {
                building = new Building(this, type, 1);
                Buildings.Add(building);
            }
            else {
                building.Level++;
            }
        }

        public decimal GetBuildingBonusMultiplier(BuildingType type) {
            return (100m + 25m * (Buildings.SingleOrDefault(b => b.Type == type)?.Level ?? 0)) / 100m;
        }

        public virtual int GetRecruitsPerDay() {
            var barracksCapacity = (Buildings.SingleOrDefault(b => b.Type == BuildingType.Barracks)?.Level ?? 0) * 10;
            if (barracksCapacity <= Archers.GetTotals() + Cavalry.GetTotals() + Footmen.GetTotals()) {
                return 0;
            }

            var hutCapacity = (Buildings.SingleOrDefault(b => b.Type == BuildingType.Huts)?.Level ?? 0) * 10;
            if (hutCapacity <= Peasants + Farmers + WoodWorkers + StoneMasons + OreMiners) {
                return 0;
            }

            var recruiting = 0;
            var totalBuildingGold = GetTotalGoldSpentOnBuildings();

            // Get recruiting for total gold spent
            recruiting += BuildingRecruitingLevels.Where(g => g <= totalBuildingGold).Count();

            // Get recruiting for defences
            recruiting += Buildings.SingleOrDefault(b => b.Type == BuildingType.Defences)?.Level ?? 0;

            return Math.Max(1, Math.Min(25, recruiting));
        }

        public int GetTotalGoldSpentOnBuildings() {
            var startingBuildings = GetStartingBuildings().ToDictionary(b => b.Type, b => b.Level);
            var buildingTotals = Buildings
                .Where(b => b.Type != BuildingType.Defences)
                .Select(b => new {
                    Definition = BuildingDefinitionFactory.Get(b.Type),
                    StartingLevel = startingBuildings.ContainsKey(b.Type) ? startingBuildings[b.Type] : 0,
                    b.Level
                })
                .Sum(b => Enumerable.Range(b.StartingLevel, b.Level - b.StartingLevel).Sum(l => b.Definition.GetNextLevelCost(l).Gold));

            return buildingTotals;
        }

        public virtual void TrainTroops(int archers, int mercenaryArchers, int cavalry, int mercenaryCavalry, int footmen, int mercenaryFootmen) {
            var troops = archers + cavalry + footmen;
            var mercenaries = mercenaryArchers + mercenaryCavalry + mercenaryFootmen;

            Archers += new Troops(archers, mercenaryArchers);
            Cavalry += new Troops(cavalry, mercenaryCavalry);
            Footmen += new Troops(footmen, mercenaryFootmen);

            Peasants -= troops;
            Resources -= (archers * ArcherTrainingCost
                + cavalry * CavalryTrainingCost
                + footmen * FootmanTrainingCost
                + mercenaries * MercenaryTrainingCost);
        }

        public virtual void UntrainTroops(int archers, int mercenaryArchers, int cavalry, int mercenaryCavalry, int footmen, int mercenaryFootmen) {
            Archers -= new Troops(archers, mercenaryArchers);
            Cavalry -= new Troops(cavalry, mercenaryCavalry);
            Footmen -= new Troops(footmen, mercenaryFootmen);

            Peasants += archers + cavalry + footmen;
        }

        public virtual void ProcessAttack(Player defender, Resources gainedResources, int attackTurns) {
            Resources += gainedResources;
            defender.Resources -= gainedResources;
            AttackTurns -= attackTurns;
        }
    }
}