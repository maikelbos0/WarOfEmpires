using WarOfEmpires.Domain.Common;

namespace WarOfEmpires.Domain.Empires {
    public sealed class BuildingDefinition {
        private ExpressionGenerator<string> _nameGenerator;
        private ExpressionGenerator<string> _descriptionGenerator;
        private ExpressionGenerator<Resources> _costGenerator;

        public BuildingDefinition(ExpressionGenerator<string> nameGenerator, ExpressionGenerator<string> descriptionGenerator, ExpressionGenerator<Resources> costGenerator) {
            _nameGenerator = nameGenerator;
            _descriptionGenerator = descriptionGenerator;
            _costGenerator = costGenerator;
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
    }
}