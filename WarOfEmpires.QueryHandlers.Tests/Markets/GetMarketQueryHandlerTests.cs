using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Markets;
using WarOfEmpires.QueryHandlers.Markets;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Markets {
    [TestClass]
    public sealed class GetMarketQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        public GetMarketQueryHandlerTests() {
            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();
            var caravans = new List<Caravan>() {
                new Caravan(player) {
                    Merchandise = new List<Merchandise>() { new Merchandise(MerchandiseType.Wood, 25000, 10) }
                },
                new Caravan(player) {
                    Merchandise = new List<Merchandise>() { new Merchandise(MerchandiseType.Ore, 25000, 10) }
                }
            };
            var id = 1;

            foreach (var caravan in caravans) {
                typeof(Caravan).GetProperty(nameof(Caravan.Id)).SetValue(caravan, id++);
            }

            user.Id.Returns(1);
            user.Status.Returns(UserStatus.Active);
            user.Email.Returns("test@test.com");

            player.User.Returns(user);
            player.GetBuildingBonus(BuildingType.Market).Returns(25000);
            player.Workers.Returns(new List<Workers>() { new Workers(WorkerType.Merchants, 7) });
            player.Caravans.Returns(caravans);

            _context.Users.Add(user);
            _context.Players.Add(player);

            foreach (var status in new[] { UserStatus.Active, UserStatus.Active, UserStatus.Active, UserStatus.Active, UserStatus.Inactive, UserStatus.New }) {
                var sellerUser = Substitute.For<User>();
                var seller = Substitute.For<Player>();

                sellerUser.Status.Returns(status);

                seller.User.Returns(sellerUser);
                seller.Caravans.Returns(new List<Caravan>() {
                    new Caravan(seller) {
                        Merchandise = new List<Merchandise>() {
                            new Merchandise(MerchandiseType.Food, 10000, 10),
                            new Merchandise(MerchandiseType.Wood, 11000, 9),
                            new Merchandise(MerchandiseType.Stone, 12000, 8),
                            new Merchandise(MerchandiseType.Ore, 13000, 7)
                        }
                    },
                    new Caravan(seller) {
                        Merchandise = new List<Merchandise>() {
                            new Merchandise(MerchandiseType.Food, 5000, 5),
                            new Merchandise(MerchandiseType.Wood, 6000, 4),
                            new Merchandise(MerchandiseType.Stone, 7000, 3),
                            new Merchandise(MerchandiseType.Ore, 8000, 2)
                        }
                    }
                });

                _context.Users.Add(sellerUser);
                _context.Players.Add(seller);
            }
        }

        [TestMethod]
        public void GetMarketQueryHandler() {
            var handler = new GetMarketQueryHandler(_context);
            var query = new GetMarketQuery("test@test.com");

            var result = handler.Execute(query);

            result.TotalMerchants.Should().Be(7);
            result.AvailableMerchants.Should().Be(5);
            result.CaravanCapacity.Should().Be(25000);
            result.AvailableCapacity.Should().Be(125000);
        }

        [TestMethod]
        public void GetMarketQueryHandler_Returns_Correct_Food() {
            var handler = new GetMarketQueryHandler(_context);
            var query = new GetMarketQuery("test@test.com");

            var result = handler.Execute(query);

            result.FoodInfo.LowestPrice.Should().Be(5);
            result.FoodInfo.AvailableAtLowestPrice.Should().Be(20000);
            result.FoodInfo.TotalAvailable.Should().Be(60000);
        }

        [TestMethod]
        public void GetMarketQueryHandler_Returns_Correct_Wood() {
            var handler = new GetMarketQueryHandler(_context);
            var query = new GetMarketQuery("test@test.com");

            var result = handler.Execute(query);

            result.WoodInfo.LowestPrice.Should().Be(4);
            result.WoodInfo.AvailableAtLowestPrice.Should().Be(24000);
            result.WoodInfo.TotalAvailable.Should().Be(93000);
        }

        [TestMethod]
        public void GetMarketQueryHandler_Returns_Correct_Stone() {
            var handler = new GetMarketQueryHandler(_context);
            var query = new GetMarketQuery("test@test.com");

            var result = handler.Execute(query);

            result.StoneInfo.LowestPrice.Should().Be(3);
            result.StoneInfo.AvailableAtLowestPrice.Should().Be(28000);
            result.StoneInfo.TotalAvailable.Should().Be(76000);
        }

        [TestMethod]
        public void GetMarketQueryHandler_Returns_Correct_Ore() {
            var handler = new GetMarketQueryHandler(_context);
            var query = new GetMarketQuery("test@test.com");

            var result = handler.Execute(query);

            result.OreInfo.LowestPrice.Should().Be(2);
            result.OreInfo.AvailableAtLowestPrice.Should().Be(32000);
            result.OreInfo.TotalAvailable.Should().Be(109000);
        }
    }
}