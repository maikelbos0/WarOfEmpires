using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Empires;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Empires {
    [TestClass]
    public sealed class GetHousingTotalsQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        public GetHousingTotalsQueryHandlerTests() {
            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();

            user.Id.Returns(1);
            user.Status.Returns(UserStatus.Active);
            user.Email.Returns("test@test.com");

            player.User.Returns(user);
            player.Buildings.Returns(new List<Building>() {
                new Building(BuildingType.Barracks, 7),
                new Building(BuildingType.Huts, 4)
            });
            player.Troops.Returns(new List<Troops>() { new Troops(TroopType.Archers, 25, 5), new Troops(TroopType.Cavalry, 5, 1), new Troops(TroopType.Footmen, 10, 2) });

            player.Peasants.Returns(7);
            player.Farmers.Returns(5);
            player.WoodWorkers.Returns(12);
            player.StoneMasons.Returns(2);
            player.OreMiners.Returns(1);
            player.SiegeEngineers.Returns(3);
            
            player.GetAvailableHousingCapacity().Returns(4);
            player.GetTheoreticalRecruitsPerDay().Returns(5);

            _context.Users.Add(user);
            _context.Players.Add(player);
        }

        [TestMethod]
        public void GetHousingTotalsQueryHandler_Returns_Correct_Values() {
            var handler = new GetHousingTotalsQueryHandler(_context);
            var query = new GetHousingTotalsQuery("test@test.com");

            var result = handler.Execute(query);

            result.BarracksCapacity.Should().Be(70);
            result.BarracksOccupancy.Should().Be(48);
            result.HutCapacity.Should().Be(40);
            result.HutOccupancy.Should().Be(30);
            result.HasHousingShortage.Should().BeTrue();
        }
    }
}