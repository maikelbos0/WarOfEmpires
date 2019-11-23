namespace WarOfEmpires.Domain.Players {
    public class Player : AggregateRoot {
        public const int RecruitingEffortStep = 24;
        public const int WorkerTrainingCost = 250;
        public const int BaseGoldProduction = 500;
        public const int BaseResourceProduction = 20;

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
        public virtual int Gold { get; protected set; } = 10000;
        public virtual int Food { get; protected set; }
        public virtual int Wood { get; protected set; }
        public virtual int Stone { get; protected set; }
        public virtual int Ore { get; protected set; }

        protected Player() {
        }

        public Player(int id, string displayName) {
            Id = id;
            DisplayName = displayName;
        }

        public int GetGoldPerWorkerPerTurn() {
            return (int)(0.5 * BaseGoldProduction);
        }

        public int GetFoodPerWorkerPerTurn() {
            return (int)(0.5 * BaseResourceProduction);
        }

        public int GetWoodPerWorkerPerTurn() {
            return (int)(0.5 * BaseResourceProduction);
        }

        public int GetStonePerWorkerPerTurn() {
            return (int)(0.5 * BaseResourceProduction);
        }

        public int GetOrePerWorkerPerTurn() {
            return (int)(0.5 * BaseResourceProduction);
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
        public void Recruit() {
            CurrentRecruitingEffort += RecruitsPerDay;

            if (CurrentRecruitingEffort > RecruitingEffortStep) {
                var newRecruits = CurrentRecruitingEffort / RecruitingEffortStep;

                CurrentRecruitingEffort -= newRecruits * RecruitingEffortStep;
                Peasants += newRecruits;
            }
        }

        public virtual void TrainWorkers(int farmers, int woodWorkers, int stoneMasons, int oreMiners) {
            var trainedPeasants = farmers + woodWorkers + stoneMasons + oreMiners;

            Farmers += farmers;
            WoodWorkers += woodWorkers;
            StoneMasons += stoneMasons;
            OreMiners += oreMiners;

            Peasants -= trainedPeasants;
            Gold -= trainedPeasants * WorkerTrainingCost;
        }

        public virtual void UntrainWorkers(int farmers, int woodWorkers, int stoneMasons, int oreMiners) {
            Farmers -= farmers;
            WoodWorkers -= woodWorkers;
            StoneMasons -= stoneMasons;
            OreMiners -= oreMiners;

            Peasants += farmers + woodWorkers + stoneMasons + oreMiners;
        }
    }
}