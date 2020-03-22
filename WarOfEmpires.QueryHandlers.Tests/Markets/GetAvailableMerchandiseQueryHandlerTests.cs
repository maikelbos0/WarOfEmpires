using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Markets;
using WarOfEmpires.QueryHandlers.Markets;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Markets {
    [TestClass]
    public sealed class GetAvailableMerchandiseQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        public GetAvailableMerchandiseQueryHandlerTests() {
            foreach (var status in new[] { UserStatus.Active, UserStatus.Active, UserStatus.Active, UserStatus.Active, UserStatus.Inactive, UserStatus.New }) {
                var user = Substitute.For<User>();
                var player = Substitute.For<Player>();

                user.Status.Returns(status);

                player.User.Returns(user);
                player.Caravans.Returns(new List<Caravan>() {
                    new Caravan(player) {
                        Merchandise = new List<Merchandise>() {
                            new Merchandise(MerchandiseType.Food, 10000, 10),
                            new Merchandise(MerchandiseType.Wood, 11000, 9),
                            new Merchandise(MerchandiseType.Stone, 12000, 8),
                            new Merchandise(MerchandiseType.Ore, 13000, 7)
                        }
                    },
                    new Caravan(player) {
                        Merchandise = new List<Merchandise>() {
                            new Merchandise(MerchandiseType.Food, 5000, 5),
                            new Merchandise(MerchandiseType.Wood, 6000, 4),
                            new Merchandise(MerchandiseType.Stone, 7000, 3),
                            new Merchandise(MerchandiseType.Ore, 8000, 2)
                        }
                    }
                });

                _context.Users.Add(user);
                _context.Players.Add(player);
            }
        }

        [TestMethod]
        public void GetAvailableMerchandiseQueryHandler_Returns_Correct_Food() {
            var handler = new GetAvailableMerchandiseQueryHandler(_context);
            var query = new GetAvailableMerchandiseQuery("test@test.com");

            var result = handler.Execute(query);

            result.FoodInfo.MinimumPrice.Should().Be(5);
            result.FoodInfo.AvailableAtMinimumPrice.Should().Be(20000);
            result.FoodInfo.TotalAvailable.Should().Be(60000);
        }

        [TestMethod]
        public void GetAvailableMerchandiseQueryHandler_Returns_Correct_Wood() {
            var handler = new GetAvailableMerchandiseQueryHandler(_context);
            var query = new GetAvailableMerchandiseQuery("test@test.com");

            var result = handler.Execute(query);

            result.WoodInfo.MinimumPrice.Should().Be(4);
            result.WoodInfo.AvailableAtMinimumPrice.Should().Be(24000);
            result.WoodInfo.TotalAvailable.Should().Be(68000);
        }

        [TestMethod]
        public void GetAvailableMerchandiseQueryHandler_Returns_Correct_Stone() {
            var handler = new GetAvailableMerchandiseQueryHandler(_context);
            var query = new GetAvailableMerchandiseQuery("test@test.com");

            var result = handler.Execute(query);

            result.StoneInfo.MinimumPrice.Should().Be(3);
            result.StoneInfo.AvailableAtMinimumPrice.Should().Be(28000);
            result.StoneInfo.TotalAvailable.Should().Be(76000);
        }

        [TestMethod]
        public void GetAvailableMerchandiseQueryHandler_Returns_Correct_Ore() {
            var handler = new GetAvailableMerchandiseQueryHandler(_context);
            var query = new GetAvailableMerchandiseQuery("test@test.com");

            var result = handler.Execute(query);

            result.OreInfo.MinimumPrice.Should().Be(2);
            result.OreInfo.AvailableAtMinimumPrice.Should().Be(32000);
            result.OreInfo.TotalAvailable.Should().Be(84000);
        }
    }
}