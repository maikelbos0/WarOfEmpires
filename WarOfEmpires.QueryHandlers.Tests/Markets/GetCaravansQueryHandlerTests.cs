using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
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
    public sealed class GetCaravansQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        public GetCaravansQueryHandlerTests() {
            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();
            var caravans = new List<Caravan>() {
                new Caravan(player) {
                    Merchandise = new List<Merchandise>() {
                        new Merchandise(MerchandiseType.Food, 1000, 10),
                        new Merchandise(MerchandiseType.Wood, 2000, 9),
                        new Merchandise(MerchandiseType.Stone, 3000, 8),
                        new Merchandise(MerchandiseType.Ore, 4000, 7),
                    }
                },
                new Caravan(player) {
                    Merchandise = new List<Merchandise>() { new Merchandise(MerchandiseType.Ore, 25000, 4) }
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
        }

        [TestMethod]
        public void GetCaravansQueryHandler_Returns_Merchant_Information() {
            var handler = new GetCaravansQueryHandler(_context);
            var query = new GetCaravansQuery("test@test.com");

            var result = handler.Execute(query);

            result.TotalMerchants.Should().Be(7);
            result.AvailableMerchants.Should().Be(5);
            result.CaravanCapacity.Should().Be(25000);
        }

        [TestMethod]
        public void GetCaravansQueryHandler_Returns_Existing_Caravans() {
            var handler = new GetCaravansQueryHandler(_context);
            var query = new GetCaravansQuery("test@test.com");

            var result = handler.Execute(query);

            result.CurrentCaravans.Should().HaveCount(2);

            result.CurrentCaravans[0].Id.Should().Be(1);
            result.CurrentCaravans[0].Date.Should().BeCloseTo(DateTime.UtcNow, 1000);
            result.CurrentCaravans[0].Food.Should().Be(1000);
            result.CurrentCaravans[0].FoodPrice.Should().Be(10);
            result.CurrentCaravans[0].Wood.Should().Be(2000);
            result.CurrentCaravans[0].WoodPrice.Should().Be(9);
            result.CurrentCaravans[0].Stone.Should().Be(3000);
            result.CurrentCaravans[0].StonePrice.Should().Be(8);
            result.CurrentCaravans[0].Ore.Should().Be(4000);
            result.CurrentCaravans[0].OrePrice.Should().Be(7);

            result.CurrentCaravans[1].Id.Should().Be(2);
            result.CurrentCaravans[1].Date.Should().BeCloseTo(DateTime.UtcNow, 1000);
            result.CurrentCaravans[1].Ore.Should().Be(25000);
            result.CurrentCaravans[1].OrePrice.Should().Be(4);            
        }
    }
}