using System.Collections.Generic;

namespace WarOfEmpires.Models.Empires {
    public sealed class ResearchModel {
        public List<ResearchViewModel> Research { get; set; }
        public List<QueuedResearchViewModel> QueuedResearch { get; set; }
        public int QueuedResearchId { get; set; }
        public string Type { get; set; }
        public string Command { get; set; }
    }
}
