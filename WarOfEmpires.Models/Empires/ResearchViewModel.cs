namespace WarOfEmpires.Models.Empires {
    public sealed class ResearchViewModel {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public decimal LevelBonus { get; set; }
        public decimal CurrentBonus { get; set; }
    }
}
