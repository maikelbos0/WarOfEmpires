namespace WarOfEmpires.Domain.Players {
    public class Player : AggregateRoot {
        public const int RecruitingEffortStep = 24;

        public virtual string DisplayName { get; set; }
        public virtual Security.User User { get; protected set; }
        public virtual int Peasants { get; set; } = 10;
        public virtual int RecruitsPerDay { get; set; } = 1;
        /// <summary>
        /// Current number of expected recruits / 24
        /// </summary>
        public virtual int CurrentRecruitingEffort { get; set; } = 0;

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