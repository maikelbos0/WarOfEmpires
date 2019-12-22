using WarOfEmpires.Domain.Common;

namespace WarOfEmpires.Domain.Empires {
    public sealed class BuildingDefinition {
        private ExpressionGenerator<string> _nameGenerator;
        private ExpressionGenerator<string> _descriptionGenerator;
        private ExpressionGenerator<Resources> _costGenerator;
        private ExpressionGenerator<int> _bonusGenerator;

        public BuildingDefinition(ExpressionGenerator<string> nameGenerator, 
            ExpressionGenerator<string> descriptionGenerator, 
            ExpressionGenerator<Resources> costGenerator,
            ExpressionGenerator<int> bonusGenerator) {

            _nameGenerator = nameGenerator;
            _descriptionGenerator = descriptionGenerator;
            _costGenerator = costGenerator;
            _bonusGenerator = bonusGenerator;
        }

        public Resources GetNextLevelCost(int currentLevel) {
            return _costGenerator.Get(currentLevel);
        }

        public string GetName(int currentLevel) {
            return _nameGenerator.Get(currentLevel);
        }

        public string GetDescription(int currentLevel) {
            return _descriptionGenerator.Get(currentLevel);
        }

        public int GetBonus(int currentLevel) {
            return _bonusGenerator.Get(currentLevel);
        }
    }
}