using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Empires;

namespace WarOfEmpires.Domain.Players {
    public class Player : AggregateRoot {
        public const int RecruitingEffortStep = 24;
        public const int BaseGoldProduction = 500;
        public const int BaseResourceProduction = 20;
        public static int[] BuildingRecruitingLevels = { 50000, 100000, 200000, 300000, 500000, 800000, 1200000, 2000000, 3000000, 5000000, 8000000, 12000000, 20000000, 30000000, 40000000, 50000000, 60000000, 70000000, 80000000, 90000000, 100000000, 110000000, 120000000, 130000000, 140000000, 150000000 };

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
        public virtual int Archers { get; protected set; }
        public virtual int MercenaryArchers { get; protected set; }
        public virtual int Cavalry { get; protected set; }
        public virtual int MercenaryCavalry { get; protected set; }
        public virtual int Footmen { get; protected set; }
        public virtual int MercenaryFootmen { get; protected set; }
        public virtual ICollection<Building> Buildings { get; protected set; } = new List<Building>();
        public virtual ICollection<Message> SentMessages { get; protected set; } = new List<Message>();
        public virtual ICollection<Message> ReceivedMessages { get; protected set; } = new List<Message>();

        protected Player() {
        }

        public Player(int id, string displayName) {
            Id = id;
            DisplayName = displayName;
        }

        public decimal GetTaxRate() {
            return (decimal)Tax / 100;
        }

        public int GetBaseGoldPerTurn() {
            return BaseGoldProduction;
        }

        public int GetBaseFoodPerTurn() {
            return (int)(BaseResourceProduction * GetBuildingResourceMultiplier(BuildingType.Farm));
        }

        public int GetBaseWoodPerTurn() {
            return (int)(BaseResourceProduction * GetBuildingResourceMultiplier(BuildingType.Lumberyard));
        }

        public int GetBaseStonePerTurn() {
            return (int)(BaseResourceProduction * GetBuildingResourceMultiplier(BuildingType.Quarry));
        }

        public int GetBaseOrePerTurn() {
            return (int)(BaseResourceProduction * GetBuildingResourceMultiplier(BuildingType.Mine));
        }

        public int GetGoldPerWorkerPerTurn() {
            return (int)(GetTaxRate() * GetBaseGoldPerTurn());
        }

        public int GetFoodPerWorkerPerTurn() {
            return (int)((1 - GetTaxRate()) * GetBaseFoodPerTurn());
        }

        public int GetWoodPerWorkerPerTurn() {
            return (int)((1 - GetTaxRate()) * GetBaseWoodPerTurn());
        }

        public int GetStonePerWorkerPerTurn() {
            return (int)((1 - GetTaxRate()) * GetBaseStonePerTurn());
        }

        public int GetOrePerWorkerPerTurn() {
            return (int)((1 - GetTaxRate()) * GetBaseOrePerTurn());
        }

        public int GetGoldPerTurn() {
            return GetGoldPerWorkerPerTurn() * (Farmers + WoodWorkers + StoneMasons + OreMiners);
        }

        public int GetFoodPerTurn() {
            return GetFoodPerWorkerPerTurn() * Farmers;
        }

        public int GetWoodPerTurn() {
            return GetWoodPerWorkerPerTurn() * WoodWorkers;
        }

        public int GetStonePerTurn() {
            return GetStonePerWorkerPerTurn() * StoneMasons;
        }

        public int GetOrePerTurn() {
            return GetOrePerWorkerPerTurn() * OreMiners;
        }

        public virtual Resources GetUpkeepPerTurn() {
            var upkeep = new Resources();

            upkeep += (Peasants + Farmers + WoodWorkers + StoneMasons + OreMiners) * PeasantUpkeep;
            upkeep += (Archers + Cavalry + Footmen) * SoldierUpkeep;
            upkeep += (MercenaryArchers + MercenaryCavalry + MercenaryFootmen) * MercenaryUpkeep;

            return upkeep;
        }

        /// <summary>
        /// Hourly function to work out the new recruiting efford and possible new peasants
        /// </summary>
        public virtual void Recruit() {
            CurrentRecruitingEffort += GetRecruitsPerDay();

            if (CurrentRecruitingEffort >= RecruitingEffortStep) {
                var newRecruits = CurrentRecruitingEffort / RecruitingEffortStep;

                CurrentRecruitingEffort -= newRecruits * RecruitingEffortStep;
                Peasants += newRecruits;
            }
        }

        /// <summary>
        /// Timed function to get the turn-based new resources and gold
        /// </summary>
        public virtual void GatherResources() {
            var upkeep = GetUpkeepPerTurn();

            if (Resources.CanAfford(upkeep)) {
                Resources = Resources - upkeep + new Resources(
                    GetGoldPerTurn(),
                    GetFoodPerTurn(),
                    GetWoodPerTurn(),
                    GetStonePerTurn(),
                    GetOrePerTurn()
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

                MercenaryArchers = 0;
                MercenaryCavalry = 0;
                MercenaryFootmen = 0;
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

        public decimal GetBuildingResourceMultiplier(BuildingType type) {
            return (100m + 25m * (Buildings.SingleOrDefault(b => b.Type == type)?.Level ?? 0)) / 100m;
        }

        public virtual int GetRecruitsPerDay() {
            int recruiting = 0;
            int totalBuildingGold = GetTotalGoldSpentOnBuildings();

            // Get recruiting for total gold spent
            recruiting += BuildingRecruitingLevels.Where(g => g <= totalBuildingGold).Count();

            return Math.Max(1, Math.Min(25, recruiting));
        }

        public int GetTotalGoldSpentOnBuildings() {
            // TODO filter out defenses when introduced
            return Buildings.Select(b => new {
                Definition = BuildingDefinitionFactory.Get(b.Type),
                b.Level
            }).Sum(b => Enumerable.Range(0, b.Level).Sum(l => b.Definition.GetNextLevelCost(l).Gold));
        }

        public void TrainTroops(int archers, int mercenaryArchers, int cavalry, int mercenaryCavalry, int footmen, int mercenaryFootmen) {
            var troops = archers + cavalry + footmen;
            var mercenaries = mercenaryArchers + mercenaryCavalry + mercenaryFootmen;

            Archers -= archers;
            MercenaryArchers -= mercenaryArchers;
            Cavalry -= cavalry;
            MercenaryCavalry -= mercenaryCavalry;
            Footmen -= footmen;
            MercenaryFootmen -= mercenaryFootmen;

            Peasants -= troops;
            Resources -= (archers * ArcherTrainingCost
                + cavalry * CavalryTrainingCost
                + footmen * FootmanTrainingCost
                + mercenaries * MercenaryTrainingCost);            
        }

        public void UntrainTroops(int archers, int mercenaryArchers, int cavalry, int mercenaryCavalry, int footmen, int mercenaryFootmen) {
            Archers -= archers;
            MercenaryArchers -= mercenaryArchers;
            Cavalry -= cavalry;
            MercenaryCavalry -= mercenaryCavalry;
            Footmen -= footmen;
            MercenaryFootmen -= mercenaryFootmen;

            Peasants += archers + cavalry + footmen;
        }
    }
}