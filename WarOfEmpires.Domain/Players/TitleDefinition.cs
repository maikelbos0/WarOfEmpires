using System;

namespace WarOfEmpires.Domain.Players {
    public sealed class TitleDefinition {
        public TitleType Type { get; }
        public int RequiredDefenceLevel { get; }
        public int RequiredSoldiers { get; }
        public int RequiredRank { get; }

        public TitleDefinition(TitleType type, int requiredDefenceLevel, int requiredSoldiers) : this(type, requiredDefenceLevel, requiredSoldiers, int.MaxValue) {
        }

        public TitleDefinition(TitleType type, int requiredDefenceLevel, int requiredSoldiers, int requiredRank) {
            Type = type;
            RequiredDefenceLevel = requiredDefenceLevel;
            RequiredSoldiers = requiredSoldiers;
            RequiredRank = requiredRank;
        }
    }
}