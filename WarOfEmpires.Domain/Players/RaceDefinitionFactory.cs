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

        // TODO set sensible bonuses
        private static RaceDefinition GenerateHumans() {
            return new RaceDefinition(
                Race.Humans, 
                "Humans are excellent producers that specialize agriculture. While overall not the strongest on the battlefield, their cavalry is unrivaled."
            );
        }

        // TODO set sensible bonuses
        private static RaceDefinition GenerateDwarves() {
            return new RaceDefinition(
                Race.Dwarves, 
                "Dwarves excel in the mines where they get their ores and stones. They are fierce in hand-to-hand combat but lack experience with the bow or on horseback."
            );
        }

        // TODO set sensible bonuses
        private static RaceDefinition GenerateElves() {
            return new RaceDefinition(
                Race.Elves,
                "Elves can cultivate woodland like no other. With this wood they make the finest bows for their extremely lethal archers."
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
