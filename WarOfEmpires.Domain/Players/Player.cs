using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Siege;

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
        public static Resources SiegeEngineerTrainingCost = new Resources(gold: 2500, wood: 250, ore: 500);
        public static Resources ArcherTrainingCost = new Resources(gold: 5000, wood: 1000, ore: 500);
        public static Resources CavalryTrainingCost = new Resources(gold: 5000, ore: 1500);
        public static Resources FootmanTrainingCost = new Resources(gold: 5000, wood: 500, ore: 1000);
        public static Resources MercenaryTrainingCost = new Resources(gold: 5000);

        public static Resources PeasantUpkeep = new Resources(food: 2);
        public static Resources SoldierUpkeep = new Resources(food: 2);
        public static Resources MercenaryUpkeep = new Resources(gold: 250, food: 2);
        public static Resources HealCostPerTroopPerTurn = new Resources(food: 2);

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
        public virtual int SiegeEngineers { get; protected set; }
        public virtual Resources Resources { get; protected set; } = new Resources(10000, 2000, 2000, 2000, 2000);
        public virtual Resources BankedResources { get; protected set; } = new Resources();
        public virtual int Tax { get; set; } = 50;
        public virtual Troops Archers { get; protected set; } = new Troops(0, 0);
        public virtual Troops Cavalry { get; protected set; } = new Troops(0, 0);
        public virtual Troops Footmen { get; protected set; } = new Troops(0, 0);
        public virtual int AttackTurns { get; protected set; } = 50;
        public virtual int BankTurns { get; protected set; } = 6;
        public virtual int Stamina { get; protected set; } = 100;
        public virtual bool HasUpkeepRunOut { get; protected set; } = false;
        public virtual ICollection<Building> Buildings { get; protected set; } = new List<Building>();
        public virtual ICollection<SiegeWeapon> SiegeWeapons { get; protected set; } = new List<SiegeWeapon>();
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

            upkeep += (Peasants + Farmers + WoodWorkers + StoneMasons + OreMiners + SiegeEngineers) * PeasantUpkeep;
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

        public virtual Resources GetBankCapacity() {
            return new Resources(
                GetBuildingBonus(BuildingType.GoldBank),
                GetBuildingBonus(BuildingType.FoodBank),
                GetBuildingBonus(BuildingType.WoodBank),
                GetBuildingBonus(BuildingType.StoneBank),
                GetBuildingBonus(BuildingType.OreBank)
            );
        }

        public virtual Resources GetAvailableBankCapacity() {
            return GetBankCapacity() - BankedResources;
        }

        public virtual Resources GetBankableResources() {
            var availableCapacity = GetAvailableBankCapacity();

            return new Resources(
                Math.Min(availableCapacity.Gold, Resources.Gold),
                Math.Min(availableCapacity.Food, Resources.Food),
                Math.Min(availableCapacity.Wood, Resources.Wood),
                Math.Min(availableCapacity.Stone, Resources.Stone),
                Math.Min(availableCapacity.Ore, Resources.Ore)
            );
        }

        public virtual void Bank() {
            var toBank = GetBankableResources();

            BankTurns--;
            Resources -= toBank;
            BankedResources += toBank;
        }

        public virtual Resources GetResourcesPerTurn() {
            return new Resources(
                GetGoldPerTurn(),
                GetFoodProduction().GetTotalProduction(),
                GetWoodProduction().GetTotalProduction(),
                GetStoneProduction().GetTotalProduction(),
                GetOreProduction().GetTotalProduction()
            );
        }

        public virtual void AddBankTurn() {
            BankTurns++;
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

            if (CanAfford(upkeep)) {
                SpendResources(upkeep);
                Resources += GetResourcesPerTurn();
            }
            else {
                Resources = Resources.SubtractSafe(upkeep, out Resources remainder);
                BankedResources = BankedResources.SubtractSafe(remainder);

                Archers = new Troops(Archers.Soldiers, 0);
                Cavalry = new Troops(Cavalry.Soldiers, 0);
                Footmen = new Troops(Footmen.Soldiers, 0);
                HasUpkeepRunOut = true;
            }
        }

        public virtual void TrainWorkers(int farmers, int woodWorkers, int stoneMasons, int oreMiners, int siegeEngineers) {
            var trainedPeasants = farmers + woodWorkers + stoneMasons + oreMiners;

            Farmers += farmers;
            WoodWorkers += woodWorkers;
            StoneMasons += stoneMasons;
            OreMiners += oreMiners;
            SiegeEngineers += siegeEngineers;

            Peasants -= trainedPeasants;
            SpendResources(trainedPeasants * WorkerTrainingCost);

            Peasants -= siegeEngineers;
            SpendResources(siegeEngineers * SiegeEngineerTrainingCost);
        }

        public virtual void UntrainWorkers(int farmers, int woodWorkers, int stoneMasons, int oreMiners, int siegeEngineers) {
            Farmers -= farmers;
            WoodWorkers -= woodWorkers;
            StoneMasons -= stoneMasons;
            OreMiners -= oreMiners;
            SiegeEngineers -= siegeEngineers;

            Peasants += farmers + woodWorkers + stoneMasons + oreMiners + siegeEngineers;
        }

        public virtual void UpgradeBuilding(BuildingType type) {
            var definition = BuildingDefinitionFactory.Get(type);
            var building = Buildings.SingleOrDefault(b => b.Type == type);

            SpendResources(definition.GetNextLevelCost(building?.Level ?? 0));

            if (building == null) {
                building = new Building(this, type, 1);
                Buildings.Add(building);
            }
            else {
                building.Level++;
            }
        }

        public virtual int GetBuildingBonus(BuildingType type) {
            var definition = BuildingDefinitionFactory.Get(type);

            return definition.GetBonus(Buildings.SingleOrDefault(b => b.Type == type)?.Level ?? 0);
        }

        public decimal GetBuildingBonusMultiplier(BuildingType type) {
            return (100m + GetBuildingBonus(type)) / 100m;
        }

        public int GetSiegeWeaponCount(SiegeWeaponType type) {
            return SiegeWeapons.SingleOrDefault(w => w.Type == type)?.Count ?? 0;
        }

        public virtual int GetAvailableBarracksCapacity() {
            return GetBuildingBonus(BuildingType.Barracks) - Archers.GetTotals() - Cavalry.GetTotals() - Footmen.GetTotals();
        }

        public virtual int GetAvailableHutCapacity() {
            return GetBuildingBonus(BuildingType.Huts) - Peasants - Farmers - WoodWorkers - StoneMasons - OreMiners - SiegeEngineers;
        }

        public virtual int GetAvailableHousingCapacity() {
            var housingCapacity = GetAvailableBarracksCapacity() + GetAvailableHutCapacity();

            if (housingCapacity < 0) {
                return 0;
            }
            else {
                return housingCapacity;
            }
        }

        public virtual int GetTheoreticalRecruitsPerDay() {
            var recruiting = 0;
            var totalBuildingGold = GetTotalGoldSpentOnBuildings();

            // Get recruiting for total gold spent
            recruiting += BuildingRecruitingLevels.Where(g => g <= totalBuildingGold).Count();

            // Get recruiting for defences
            recruiting += GetBuildingBonus(BuildingType.Defences);

            // Adjust for havnig not enough soldiers
            recruiting -= GetSoldierRecruitsPenalty();

            if (recruiting > 25) {
                return 25;
            }
            else if (recruiting < 1) {
                return 1;
            }
            else {
                return recruiting;
            }
        }

        public virtual int GetSoldierRecruitsPenalty() {
            var soldiers = Archers.Soldiers + Cavalry.Soldiers + Footmen.Soldiers;
            var peasants = Peasants + Farmers + WoodWorkers + StoneMasons + OreMiners + SiegeEngineers;
            var ratio = 1.0m * soldiers / (soldiers + peasants);

            if (ratio >= 0.5m) {
                return 0;
            }
            else if (ratio >= 0.45m) {
                return 1;
            }
            else if (ratio >= 0.4m) {
                return 2;
            }
            else {
                return 3;
            }
        }

        public virtual int GetRecruitsPerDay() {
            var housingCapacity = GetAvailableHousingCapacity();
            var recruiting = GetTheoreticalRecruitsPerDay();

            return Math.Min(housingCapacity, recruiting);
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
            SpendResources(archers * ArcherTrainingCost
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

        public virtual void HealTroops(int staminaToHeal) {
            var troops = Archers.GetTotals() + Cavalry.GetTotals() + Footmen.GetTotals();
            Stamina += staminaToHeal;
            SpendResources(troops * HealCostPerTroopPerTurn * staminaToHeal);
        }

        public virtual void ProcessAttack(Player defender, Resources gainedResources, int attackTurns) {
            Resources += gainedResources;
            defender.Resources -= gainedResources;
            AttackTurns -= attackTurns;

            CheckUpkeep();
        }

        // TODO make sure this function is called whenever resources get added
        public virtual void CheckUpkeep() {
            if (HasUpkeepRunOut && CanAfford(GetUpkeepPerTurn())) {
                HasUpkeepRunOut = false;
            }
        }

        public virtual Resources GetTotalResources() {
            return Resources + BankedResources;
        }

        public virtual bool CanAfford(Resources resources) {
            return GetTotalResources().CanAfford(resources);
        }

        public virtual void SpendResources(Resources resources) {
            Resources = Resources.SubtractSafe(resources, out Resources remainder);
            BankedResources -= remainder;
        }


        public virtual void BuildSiege(SiegeWeaponType type, int count) {
            var definition = SiegeWeaponDefinitionFactory.Get(type);
            var weapon = SiegeWeapons.SingleOrDefault(b => b.Type == type);

            SpendResources(definition.Cost * count);

            if (weapon == null) {
                weapon = new SiegeWeapon(this, type, count);
                SiegeWeapons.Add(weapon);
            }
            else {
                weapon.Count += count;
            }
        }

        public virtual void DiscardSiege(SiegeWeaponType type, int count) {
            var weapon = SiegeWeapons.Single(b => b.Type == type);

            weapon.Count -= count;
        }
    }
}