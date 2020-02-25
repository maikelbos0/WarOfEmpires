using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.QueryHandlers.Empires;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Empires {
    [TestClass]
    public sealed class GetBuildingUpgradesQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly ResourcesMap _resourcesMap = new ResourcesMap();

        public GetBuildingUpgradesQueryHandlerTests() {
            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();

            user.Id.Returns(1);
            user.Status.Returns(UserStatus.Active);
            user.Email.Returns("test@test.com");

            player.User.Returns(user);
            player.Buildings.Returns(new List<Building>() {
                new Building(BuildingType.Lumberyard, 2)
            });

            _context.Users.Add(user);
            _context.Players.Add(player);
        }

        [TestMethod]
        public void GetBuildingUpgradesQueryHandler_Succeeds_For_Existing_Building() {
            var handler = new GetBuildingUpgradesQueryHandler(_context, _resourcesMap);
            var query = new GetBuildingUpgradesQuery("test@test.com", "Lumberyard");

            var result = handler.Execute(query);

            result.Name.Should().Be("Lumberyard (level 2)");
            result.Upgrades.Count.Should().Be(15);

            result.Upgrades.First().Bonus.Should().Be(75);
            result.Upgrades.First().Name.Should().Be("Lumberyard (level 3)");
            result.Upgrades.First().Resources.Gold.Should().Be(50000);
            result.Upgrades.First().Resources.Food.Should().Be(0);
            result.Upgrades.First().Resources.Wood.Should().Be(5000);
            result.Upgrades.First().Resources.Stone.Should().Be(2500);
            result.Upgrades.First().Resources.Ore.Should().Be(1250);

            result.Upgrades.Last().Bonus.Should().Be(425);
            result.Upgrades.Last().Name.Should().Be("Lumberyard (level 17)");
            result.Upgrades.Last().Resources.Gold.Should().Be(30000000);
            result.Upgrades.Last().Resources.Food.Should().Be(0);
            result.Upgrades.Last().Resources.Wood.Should().Be(3000000);
            result.Upgrades.Last().Resources.Stone.Should().Be(1500000);
            result.Upgrades.Last().Resources.Ore.Should().Be(750000);
        }

        [TestMethod]
        public void GetBuildingUpgradesQueryHandler_Succeeds_For_Nonexistent_Building() {
            var handler = new GetBuildingUpgradesQueryHandler(_context, _resourcesMap);
            var query = new GetBuildingUpgradesQuery("test@test.com", "Farm");

            var result = handler.Execute(query);

            result.Name.Should().Be("Farm (level 0)");
            result.Upgrades.Count.Should().Be(15);

            result.Upgrades.First().Bonus.Should().Be(25);
            result.Upgrades.First().Name.Should().Be("Farm (level 1)");
            result.Upgrades.First().Resources.Gold.Should().Be(20000);
            result.Upgrades.First().Resources.Food.Should().Be(0);
            result.Upgrades.First().Resources.Wood.Should().Be(2000);
            result.Upgrades.First().Resources.Stone.Should().Be(1000);
            result.Upgrades.First().Resources.Ore.Should().Be(500);

            result.Upgrades.Last().Bonus.Should().Be(375);
            result.Upgrades.Last().Name.Should().Be("Farm (level 15)");
            result.Upgrades.Last().Resources.Gold.Should().Be(12000000);
            result.Upgrades.Last().Resources.Food.Should().Be(0);
            result.Upgrades.Last().Resources.Wood.Should().Be(1200000);
            result.Upgrades.Last().Resources.Stone.Should().Be(600000);
            result.Upgrades.Last().Resources.Ore.Should().Be(300000);
        }
    }
}