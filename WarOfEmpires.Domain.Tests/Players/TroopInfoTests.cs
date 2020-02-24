using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Players {
    [TestClass]
    public sealed class TroopInfoTests {
        [TestMethod]
        public void TroopInfo_GetTotalAttack_Succeeds() {
            var troopInfo = new TroopInfo(new Troops(TroopType.Archers, 15, 5), 60, 40, 2.25m, 2.5m, 1.5m);

            troopInfo.GetTotalAttack().Should().Be(20 * (int)(60 * 2.25m * 2.5m));
        }

        [TestMethod]
        public void TroopInfo_GetTotalDefense_Succeeds() {
            var troopInfo = new TroopInfo(new Troops(TroopType.Archers, 15, 5), 60, 40, 2.25m, 2.5m, 1.5m);

            troopInfo.GetTotalDefense().Should().Be(20 * (int)(40 * 2.25m * 1.5m));
        }
    }
}