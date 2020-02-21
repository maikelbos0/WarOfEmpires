using WarOfEmpires.Domain.Common;

namespace WarOfEmpires.Domain.Empires {
    public sealed class BuildingDefinition {
        private readonly ExpressionGenerator<string> _nameGenerator;
        private readonly ExpressionGenerator<string> _descriptionGenerator;
        private readonly ExpressionGenerator<Resources> _costGenerator;
        private readonly ExpressionGenerator<int> _bonusGenerator;

        public BuildingType Type { get; }

        public BuildingDefinition(BuildingType type,
            ExpressionGenerator<string> nameGenerator, 
            ExpressionGenerator<string> descriptionGenerator, 
            ExpressionGenerator<Resources> costGenerator,
            ExpressionGenerator<int> bonusGenerator) {

            Type = type;
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