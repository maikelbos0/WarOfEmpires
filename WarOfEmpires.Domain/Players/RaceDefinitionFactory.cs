using System.Collections.Generic;
using System.Linq;

namespace WarOfEmpires.Domain.Players {
    public static class RaceDefinitionFactory {
        private readonly static Dictionary<Race, RaceDefinition> _races = new Dictionary<Race, RaceDefinition>();

        static RaceDefinitionFactory() {
            foreach (var definition in new[] {
                GenerateHumans(), GenerateDwarves(), GenerateElves()
            }) {
                _races.Add(definition.Race, definition);
            }
        }

        private static RaceDefinition GenerateHumans() {
            return new RaceDefinition(
                Race.Humans, 
                "Humans are excellent producers that specialize agriculture. While overall not the strongest on the battlefield, their cavalry is unrivaled.",
                farmerModifier: 1.3M,
                woodWorkerModifier: 1.1M,
                archerModifier: 0.9M,
                cavalryModifier: 1.1M,
                footmenModifier: 0.8M
            );
        }

        private static RaceDefinition GenerateDwarves() {
            return new RaceDefinition(
                Race.Dwarves, 
                "Dwarves excel in the mines where they get their ores and stones. They are fierce in hand-to-hand combat but lack experience with the bow or on horseback.",
                farmerModifier: 0.8M,
                woodWorkerModifier: 0.8M,
                stoneMasonModifier: 1.2M,
                oreMinerModifier: 1.2M,
                archerModifier: 0.9M,
                cavalryModifier: 0.9M,
                footmenModifier: 1.3M
            );
        }

        private static RaceDefinition GenerateElves() {
            return new RaceDefinition(
                Race.Elves,
                "Elves can cultivate woodland like no other. With this wood they make the finest bows for their extremely lethal archers.",
                farmerModifier: 1.1M,
                woodWorkerModifier: 1.3M,
                stoneMasonModifier: 0.8M,
                oreMinerModifier: 0.8M,
                archerModifier: 1.3M,
                footmenModifier: 0.8M
            );
        }

        public static RaceDefinition Get(Race race) {
            return _races[race];
        }

        public static List<RaceDefinition> GetAll() {
            return _races.Values.ToList();
        }
    }
}
