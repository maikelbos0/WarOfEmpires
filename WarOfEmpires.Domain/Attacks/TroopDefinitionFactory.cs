using System.Collections.Generic;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Empires;

namespace WarOfEmpires.Domain.Attacks {
    public static class TroopDefinitionFactory {
        private static readonly Dictionary<TroopType, TroopDefinition> _troops = new();

        static TroopDefinitionFactory() {
            foreach (var definition in new[] { GenerateArchers(), GenerateCavalry(), GenerateFootmen() }) {
                _troops.Add(definition.Type, definition);
            }
        }
        
        private static TroopDefinition GenerateArchers() {
            return new TroopDefinition(TroopType.Archers, BuildingType.ArcheryRange, 50, 30, new Resources(gold: 5000, wood: 1000, ore: 500));
        }

        private static TroopDefinition GenerateCavalry() {
            return new TroopDefinition(TroopType.Cavalry, BuildingType.CavalryRange, 45, 35, new Resources(gold: 5000, ore: 1500));
        }

        private static TroopDefinition GenerateFootmen() {
            return new TroopDefinition(TroopType.Footmen, BuildingType.FootmanRange, 40, 40, new Resources(gold: 5000, wood: 500, ore: 1000));
        }

        public static TroopDefinition Get(TroopType type) {
            return _troops[type];
        }
    }
}