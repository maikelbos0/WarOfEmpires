using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Empires;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Empires {
    [TestClass]
    public sealed class GetBuildingTotalsQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        public GetBuildingTotalsQueryHandlerTests() {
            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();

            user.Id.Returns(1);
            user.Status.Returns(UserStatus.Active);
            user.Email.Returns("test@test.com");

            player.User.Returns(user);
            player.Buildings.Returns(new List<Building>() {
                new Building(player, BuildingType.Farm, 4),
                new Building(player, BuildingType.Lumberyard, 8),
                new Building(player, BuildingType.Quarry, 2),
                new Building(player, BuildingType.Mine, 2)
            });

            _context.Users.Add(user);
            _context.Players.Add(player);
        }

        [TestMethod]
        public void GetBuildingTotalsQueryHandler_Returns_Correct_Values() {
            var handler = new GetBuildingTotalsQueryHandler(_context);
            var query = new GetBuildingTotalsQuery("test@test.com");

            var result = handler.Execute(query);

            result.TotalGoldSpent.Should().Be(1480000);
            result.NextRecruitingLevel.Should().Be(2000000);
        }
    }
}