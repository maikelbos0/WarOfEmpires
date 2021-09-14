namespace WarOfEmpires.Models.Empires {
    public sealed class QueuedResearchViewModel : EntityViewModel {
        public string Type { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public int CompletedResearchTime { get; set; }
        public int NeededResearchTime { get; set; }
    }
}
