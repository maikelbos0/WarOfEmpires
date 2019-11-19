namespace WarOfEmpires.Domain.Players {
    public class Player : AggregateRoot {
        public const int RecruitingEffortStep = 24;

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
    }
}