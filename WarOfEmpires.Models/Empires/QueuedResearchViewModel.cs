using System;

namespace WarOfEmpires.Models.Empires {
    public sealed class QueuedResearchViewModel : EntityViewModel {
        public string Type { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public long ResearchTime { get; set; }
        public long CompletedResearchTime { get; set; }
        public long NeededResearchTime { get; set; }
        public TimeSpan NeededTime { get; set; }
    }
}
