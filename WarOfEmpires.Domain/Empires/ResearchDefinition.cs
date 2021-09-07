namespace WarOfEmpires.Domain.Empires {
    public sealed class ResearchDefinition {
        public ResearchType Type { get; private set; }
        public decimal LevelBonus { get; private set; }
        public string Description { get; private set; }

        public ResearchDefinition(ResearchType type, decimal levelBonus, string description) {
            Type = type;
            LevelBonus = levelBonus;
            Description = description;
        }
    }
}
