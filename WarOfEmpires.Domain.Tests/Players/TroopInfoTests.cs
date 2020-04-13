using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Players {
    [TestClass]
    public sealed class TroopInfoTests {
        [TestMethod]
        public void TroopInfo_GetTotalAttack_Succeeds() {
            var troopInfo = new TroopInfo(new Troops(TroopType.Archers, 15, 5), 50, 30, 1.5m, 1.6m, 1.2m, 0);

            troopInfo.GetTotalAttack().Should().Be(20 * (int)(50 * 1.5m * 1.6m));
        }

        [TestMethod]
        public void TroopInfo_GetTotalDefense_Succeeds() {
            var troopInfo = new TroopInfo(new Troops(TroopType.Archers, 15, 5), 50, 30, 1.5m, 1.6m, 1.2m, 0);

            troopInfo.GetTotalDefense().Should().Be(20 * (int)(30 * 1.5m * 1.2m));
        }

        [DataTestMethod]
        [DataRow(15, 5, 0, 1, 40, DisplayName = "No coverage, no defence")]
        [DataRow(15, 5, 0, 0.5, 20, DisplayName = "No coverage, half defence")]
        [DataRow(15, 5, 10, 1, 40, DisplayName = "Half coverage, no defence")]
        [DataRow(15, 5, 10, 0.5, 25, DisplayName = "Half coverage, half defence")]
        [DataRow(15, 5, 20, 1, 40, DisplayName = "Full coverage, no defence")]
        [DataRow(15, 5, 20, 0.5, 30, DisplayName = "Full coverage, half defence")]
        [DataRow(15, 5, 25, 0.5, 30, DisplayName = "Extra coverage, half defence")]
        public void TroopInfo_GetTotalAttack_With_Modifier_Succeeds(int soldiers, int mercenaries, int siegeCoverage, double defenceModifier, long expectedResult) {
            var troopInfo = new TroopInfo(new Troops(TroopType.Archers, soldiers, mercenaries), 2, 1, 1m, 1m, 1m, siegeCoverage);

            troopInfo.GetTotalAttack(defenceModifier).Should().Be(expectedResult);
        }
    }
}