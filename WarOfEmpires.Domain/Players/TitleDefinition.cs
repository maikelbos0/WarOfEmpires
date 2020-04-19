namespace WarOfEmpires.Domain.Players {
    public sealed class TitleDefinition {
        public TitleType Type { get; }
        public int RequiredDefenceLevel { get; }
        public int RequiredSoldiers { get; }

        public TitleDefinition(TitleType type, int requiredDefenceLevel, int requiredSoldiers) {
            Type = type;
            RequiredDefenceLevel = requiredDefenceLevel;
            RequiredSoldiers = requiredSoldiers;
        }
    }
}