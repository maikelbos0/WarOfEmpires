namespace WarOfEmpires.Models.Empires {
    public sealed class QueuedResearchViewModel : EntityViewModel {
        public int Priority { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }
}
