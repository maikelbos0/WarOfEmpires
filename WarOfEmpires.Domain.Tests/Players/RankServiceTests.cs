using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Tests.Players {
    [TestClass]
    public sealed class RankServiceTests {
        [TestMethod]
        public void RankService_GetPoints_Succeeds() {
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

            service.GetPoints(player).Should().Be((50 + 250 + 20) * 10
                + (1000 + 250) * ((int)(50 * 1.2 * 1.4) + (int)(30 * 1.4)) * 0.1
                + (10 + 10) * ((int)(40 * 1.2) + 40) * 0.1
                + 10 * 1000);
        }

        [DataTestMethod]
        [DataRow(0, 40, null, TitleType.PeasantLeader)]
        [DataRow(1, 40, null, TitleType.BanditLeader)]
        [DataRow(2, 40, null, TitleType.WarbandLeader)]
        [DataRow(3, 40, null, TitleType.SubChieftain)]
        [DataRow(15, 1200, 2, TitleType.SubChieftain)]
        [DataRow(15, 1200, 1, TitleType.GrandOverlord)]
        public void RankService_GetTitle_Succeeds(int defenceLevel, int soldiers, int? rank, TitleType expectedTitle) {
            var player = new Player(0, "test@test.com");

            player.Troops.Add(new Troops(TroopType.Archers, soldiers, 250));
            player.Buildings.Add(new Building(BuildingType.Defences, defenceLevel));

            if (rank.HasValue) {
                player.UpdateRank(rank.Value, TitleType.PeasantLeader);
            }

            var service = new RankService();

            service.GetTitle(player).Should().Be(expectedTitle);
        }

        public Player CreatePlayer(int soldierCount, int defenceLevel) {
            var player = Substitute.For<Player>();
            var troops = new Troops(TroopType.Archers, soldierCount, 0);

            player.Workers.Returns(new List<Workers>());
            player.Troops.Returns(new List<Troops>() { troops });
            player.GetTroopInfo(Arg.Any<TroopType>()).Returns(new TroopInfo(troops, 50, 30, 1, 1, 1, 0));
            player.GetBuildingBonus(BuildingType.Defences).Returns(defenceLevel);

            return player;
        }

        [TestMethod]
        public void RankService_Update_Succeeds() {
            var players = new List<Player>() {
                CreatePlayer(20, 1),
                CreatePlayer(300, 0),
                CreatePlayer(10, 0),
                CreatePlayer(30, 2)
            };

            var service = new RankService();

            service.Update(players);

            players[0].Received().UpdateRank(3, TitleType.BanditLeader);
            players[1].Received().UpdateRank(1, TitleType.PeasantLeader);
            players[2].Received().UpdateRank(4, TitleType.PeasantLeader);
            players[3].Received().UpdateRank(2, TitleType.WarbandLeader);
        }
    }
}