using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Empires;

namespace WarOfEmpires.Domain.Players {
    public class Player : AggregateRoot {
        public const int RecruitingEffortStep = 24;
        public const int BaseGoldProduction = 500;
        public const int BaseResourceProduction = 20;
        public static Resources WorkerTrainingCost = new Resources(gold: 250);
        
        public virtual string DisplayName { get; set; }
        public virtual Security.User User { get; protected set; }
        public virtual int RecruitsPerDay { get; protected set; } = 1;
        /// <summary>
        /// Current number of expected recruits / 24
        /// </summary>
        public virtual int CurrentRecruitingEffort { get; protected set; } = 0;
        public virtual int Peasants { get; protected set; } = 10;
        public virtual int Farmers { get; protected set; }
        public virtual int WoodWorkers { get; protected set; }
        public virtual int StoneMasons { get; protected set; }
        public virtual int OreMiners { get; protected set; }
        public virtual Resources Resources { get; protected set; } = new Resources(10000, 0, 0, 0, 0);
        public virtual int Tax { get; set; } = 50;
        public virtual ICollection<Building> Buildings { get; protected set; } = new List<Building>();

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
            return BaseResourceProduction;
        }

        public int GetBaseWoodPerTurn() {
            return BaseResourceProduction;
        }

        public int GetBaseStonePerTurn() {
            return BaseResourceProduction;
        }

        public int GetBaseOrePerTurn() {
            return BaseResourceProduction;
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

        /// <summary>
        /// Hourly function to work out the new recruiting efford and possible new peasants
        /// </summary>
        public virtual void Recruit() {
            CurrentRecruitingEffort += RecruitsPerDay;

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
            Resources += new Resources(
                GetGoldPerTurn(),
                GetFoodPerTurn(),
                GetWoodPerTurn(),
                GetStonePerTurn(),
                GetOrePerTurn()
            );
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
    }
}