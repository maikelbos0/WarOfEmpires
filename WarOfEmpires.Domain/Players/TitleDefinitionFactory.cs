using System;
using System.Collections.Generic;
using System.Linq;

namespace WarOfEmpires.Domain.Players {
    public static class TitleDefinitionFactory {
        private readonly static Dictionary<TitleType, TitleDefinition> _titles = new Dictionary<TitleType, TitleDefinition>();

        static TitleDefinitionFactory() {
            foreach (var definition in new[] {
                GeneratePeasantLeader(), GenerateBanditLeader(), GenerateWarbandLeader(), GenerateSubChieftain(),
                GenerateChief(), GenerateWarlord(), GenerateBaron(), GenerateViscount(),
                GenerateEarl(), GenerateMarquis(), GenerateDuke(), GeneratePrince(),
                GenerateKing(), GenerateEmperor(), GenerateOverlord(), GenerateGrandOverlord()
            }) {
                _titles.Add(definition.Type, definition);
            }
        }

        private static TitleDefinition GeneratePeasantLeader() {
            return new TitleDefinition(TitleType.PeasantLeader, 0, 0);
        }

        private static TitleDefinition GenerateBanditLeader() {
            return new TitleDefinition(TitleType.BanditLeader, 1, 10);
        }

        private static TitleDefinition GenerateWarbandLeader() {
            return new TitleDefinition(TitleType.WarbandLeader, 2, 30);
        }

        private static TitleDefinition GenerateSubChieftain() {
            return new TitleDefinition(TitleType.SubChieftain, 3, 60);
        }

        private static TitleDefinition GenerateChief() {
            return new TitleDefinition(TitleType.Chief, 4, 100);
        }

        private static TitleDefinition GenerateWarlord() {
            return new TitleDefinition(TitleType.Warlord, 5, 150);
        }

        private static TitleDefinition GenerateBaron() {
            return new TitleDefinition(TitleType.Baron, 6, 210);
        }

        private static TitleDefinition GenerateViscount() {
            return new TitleDefinition(TitleType.Viscount, 7, 280);
        }

        private static TitleDefinition GenerateEarl() {
            return new TitleDefinition(TitleType.Earl, 8, 360);
        }

        private static TitleDefinition GenerateMarquis() {
            return new TitleDefinition(TitleType.Marquis, 9, 450);
        }

        private static TitleDefinition GenerateDuke() {
            return new TitleDefinition(TitleType.Duke, 10, 550);
        }

        private static TitleDefinition GeneratePrince() {
            return new TitleDefinition(TitleType.Prince, 11, 660);
        }

        private static TitleDefinition GenerateKing() {
            return new TitleDefinition(TitleType.King, 12, 780);
        }

        private static TitleDefinition GenerateEmperor() {
            return new TitleDefinition(TitleType.Emperor, 13, 910);
        }

        private static TitleDefinition GenerateOverlord() {
            return new TitleDefinition(TitleType.Overlord, 14, 1050);
        }

        private static TitleDefinition GenerateGrandOverlord() {
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