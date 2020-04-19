using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Players {
    [TestClass]
    public sealed class RankServiceTests {
        [TestMethod]
        public void RankService_GetRankPoints_Succeeds() {
            var player = new Player(0, "test@test.com");

            player.Workers.Add(new Workers(WorkerType.Farmers, 50));
            player.Workers.Add(new Workers(WorkerType.WoodWorkers, 250));
            player.Workers.Add(new Workers(WorkerType.Merchants, 20));

            player.Troops.Add(new Troops(TroopType.Archers, 1000, 250));
            player.Troops.Add(new Troops(TroopType.Footmen, 10, 10));

            player.Buildings.Add(new Building(BuildingType.Defences, 10));
            player.Buildings.Add(new Building(BuildingType.Forge, 2));
            player.Buildings.Add(new Building(BuildingType.ArcheryRange, 4));

            var service = new RankService();

            service.GetRankPoints(player).Should().Be((50 + 250 + 20) * 10
                + (1000 + 250) * ((int)(50 * 1.2 * 1.4) + (int)(30 * 1.4)) * 0.1
                + (10 + 10) * ((int)(40 * 1.2) + 40) * 0.1
                + 10 * 1000);
        }
    }
}