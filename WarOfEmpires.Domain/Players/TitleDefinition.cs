using System;

namespace WarOfEmpires.Domain.Players {
    public sealed class TitleDefinition {
        public TitleType Type { get; }
        public int RequiredDefenceLevel { get; }
        public int RequiredSoldiers { get; }
        public Func<Player, bool> MeetsAdditionalRequirements { get; }

        public TitleDefinition(TitleType type, int requiredDefenceLevel, int requiredSoldiers) : this(type, requiredDefenceLevel, requiredSoldiers, player => true) {
        }

        public TitleDefinition(TitleType type, int requiredDefenceLevel, int requiredSoldiers, Func<Player, bool> meetsAdditionalRequirements) {
            Type = type;
            RequiredDefenceLevel = requiredDefenceLevel;
            RequiredSoldiers = requiredSoldiers;
            MeetsAdditionalRequirements = meetsAdditionalRequirements;
        }
    }
}