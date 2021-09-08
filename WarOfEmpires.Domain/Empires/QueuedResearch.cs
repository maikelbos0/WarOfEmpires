using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Empires {
    public class QueuedResearch : Entity {
        public virtual Player Player { get; protected set; }
        public virtual int Priority { get; protected set; }
        public virtual ResearchType Type { get; protected set; }
        public virtual long CompletedResearchTime { get; protected set; }

        protected QueuedResearch() { }

        public QueuedResearch(Player player, int priority, ResearchType type) {
            Player = player;
            Priority = priority;
            Type = type;
        }

        public void ProcessTurn() {
            /*
             * Logic to add completed research, check if it's enough when comparing to player's current completed research, and add completed research and remove this if it's completed
             */
        }
    }
}
