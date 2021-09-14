using System.Linq;
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

        public void ProcessTurn(long researchTime) {
            CompletedResearchTime += researchTime;

            var research = Player.Research.SingleOrDefault(r => r.Type == Type);

            if (CompletedResearchTime >= ResearchTimeCalculator.GetResearchTime(Player.Research.Sum(r => r.Level), research?.Level ?? 0)) {
                if (research == null) {
                    research = new Research(Type);
                    Player.Research.Add(research);
                }

                research.Level++;
                Player.RemoveQueuedResearch(this);
            }
        }
    }
}
