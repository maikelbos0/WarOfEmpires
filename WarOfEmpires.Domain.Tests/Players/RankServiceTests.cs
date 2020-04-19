using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        public Player CreatePlayer(string name, int soldierCount) {
            var player = new Player(0, name);

            player.Troops.Add(new Troops(TroopType.Archers, soldierCount, 0));

            return player;
        }

        [TestMethod]
        public void RankService_Update_Succeeds() {
            var players = new List<Player>() {
                CreatePlayer("Third", 20),
                CreatePlayer("First", 40),
                CreatePlayer("Fourth", 10),
                CreatePlayer("Second", 30)
            };

            var service = new RankService();

            service.Update(players);

            players[0].Rank.Should().Be(3);
            players[1].Rank.Should().Be(1);
            players[2].Rank.Should().Be(4);
            players[3].Rank.Should().Be(2);
        }
    }
}