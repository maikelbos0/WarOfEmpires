using WarOfEmpires.Domain.Common;

namespace WarOfEmpires.Domain.Empires {
    public sealed class Building {
        private ExpressionGenerator<string> NameGenerator { get; set; }
        private ExpressionGenerator<Resources> CostGenerator { get; set; }

        public Building(ExpressionGenerator<string> nameGenerator, ExpressionGenerator<Resources> costGenerator) {
            NameGenerator = nameGenerator;
            CostGenerator = costGenerator;
        }

        public Resources GetNextLevelCost(int currentLevel) {
            return CostGenerator.Get(currentLevel);
        }

        public string GetName(int currentLevel) {
            return NameGenerator.Get(currentLevel);
        }
    }
}