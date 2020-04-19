using System;
using System.Collections.Generic;
using System.Linq;

namespace WarOfEmpires.Domain.Players {
    public static class TitleDefinitionFactory {
        private readonly static Dictionary<TitleType, TitleDefinition> _titles = new Dictionary<TitleType, TitleDefinition>();

        static TitleDefinitionFactory() {
            foreach (var definition in new[] {
                GeneratePeasantLeader()
            }) {
                _titles.Add(definition.Type, definition);
            }
        }

        private static TitleDefinition GeneratePeasantLeader() {
            return new TitleDefinition(TitleType.PeasantLeader, 0, 0);
        }

        public static TitleDefinition Get(TitleType type) {
            return _titles[type];
        }

        public static List<TitleDefinition> GetAll() {
            return _titles.Values.OrderBy(t => t.Type).ToList();
        }
    }
}