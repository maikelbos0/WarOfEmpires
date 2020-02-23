using System.Collections.Generic;
using WarOfEmpires.Domain.Common;

namespace WarOfEmpires.Domain.Attacks {
    public static class TroopDefinitionFactory {
        private static readonly Dictionary<TroopType, TroopDefinition> _troops = new Dictionary<TroopType, TroopDefinition>();

        static TroopDefinitionFactory() {
            foreach (var definition in new[] { GenerateArchers(), GenerateCavalry(), GenerateFootmen() }) {
                _troops.Add(definition.Type, definition);
            }
        }

        private static TroopDefinition GenerateArchers() {
            return new TroopDefinition(TroopType.Archers, new Resources(gold: 5000, wood: 1000, ore: 500), "Archers");
        }

        private static TroopDefinition GenerateCavalry() {
            return new TroopDefinition(TroopType.Cavalry, new Resources(gold: 5000, ore: 1500), "Cavalry");
        }

        private static TroopDefinition GenerateFootmen() {
            return new TroopDefinition(TroopType.Footmen, new Resources(gold: 5000, wood: 500, ore: 1000), "Footmen");
        }

        public static TroopDefinition Get(TroopType type) {
            return _troops[type];
        }
    }
}