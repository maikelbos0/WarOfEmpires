using System;
using System.Collections.Generic;
using System.Linq;

namespace WarOfEmpires.Domain.Players {
    public static class TitleDefinitionFactory {
        private readonly static Dictionary<TitleType, TitleDefinition> _titles = new Dictionary<TitleType, TitleDefinition>();

        static TitleDefinitionFactory() {
            foreach (var definition in new[] {
                GeneratePeasantLeader(), GenerateBanditLeader(), GenerateWarbandLeader(), GenerateSubChieftain(),
                GenerateGrandOverlord()
            }) {
                _titles.Add(definition.Type, definition);
            }
        }

        private static TitleDefinition GeneratePeasantLeader() {
            return new TitleDefinition(TitleType.PeasantLeader, 0, 0);
        }

        private static TitleDefinition GenerateBanditLeader() {
            return new TitleDefinition(TitleType.BanditLeader, 1, 15);
        }

        private static TitleDefinition GenerateWarbandLeader() {
            return new TitleDefinition(TitleType.WarbandLeader, 2, 25);
        }

        private static TitleDefinition GenerateSubChieftain() {
            return new TitleDefinition(TitleType.SubChieftain, 3, 40);
        }

        private static TitleDefinition GenerateGrandOverlord() {
            // Number of soldiers is provisional
            return new TitleDefinition(TitleType.GrandOverlord, 15, 1200, player => player.Rank == 1);
        }

        public static TitleDefinition Get(TitleType type) {
            return _titles[type];
        }

        public static List<TitleDefinition> GetAll() {
            return _titles.Values.ToList();
        }
    }
}