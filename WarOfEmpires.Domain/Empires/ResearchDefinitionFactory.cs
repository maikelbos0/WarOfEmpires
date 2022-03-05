using System.Collections.Generic;

namespace WarOfEmpires.Domain.Empires {
    public static class ResearchDefinitionFactory {
        private static readonly Dictionary<ResearchType, ResearchDefinition> _researches = new();

        private static ResearchDefinition GenerateEfficiency() {
            return new ResearchDefinition(
                ResearchType.Efficiency,
                0.05M,
                "Efficiency allows each of your workers to produce more resources"
            );
        }

        private static ResearchDefinition GenerateCommerce() {
            return new ResearchDefinition(
                ResearchType.Commerce,
                0.05M,
                "Commerce increases the gold income of your workers"
            );
        }

        private static ResearchDefinition GenerateTactics() {
            return new ResearchDefinition(
                ResearchType.Tactics,
                0.05M,
                "Tactics increases the number of casualties your army inflicts on the battlefield"
            );
        }

        private static ResearchDefinition GenerateCombatMedicine() {
            return new ResearchDefinition(
                ResearchType.CombatMedicine,
                -0.04M,
                "Combat medicine lowers the number of casualties your army suffers in battle"
            );
        }

        private static ResearchDefinition GenerateSafeStorage() {
            return new ResearchDefinition(
                ResearchType.SafeStorage,
                0.06M,
                "Safe storage makes your workers store a percentage of their production in your banks automatically"
            );
        }

        private static ResearchDefinition GenerateRegency() {
            return new ResearchDefinition(
                ResearchType.Regency,
                0.05M,
                "Regents hire back a percentage of the mercenaries you lose when you are attacked"
            );
        }

        static ResearchDefinitionFactory() {
            foreach (var definition in new[] {
                GenerateEfficiency(), GenerateCommerce(),
                GenerateTactics(), GenerateCombatMedicine(),
                GenerateSafeStorage(), GenerateRegency()
            }) {
                _researches.Add(definition.Type, definition);
            }
        }

        public static ResearchDefinition Get(ResearchType type) {
            return _researches[type];
        }
    }
}
