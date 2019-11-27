using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Empires;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Empires {
    [TestClass]
    public sealed class GetBuildingQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly ResourcesMap _resourcesMap = new ResourcesMap();

        public GetBuildingQueryHandlerTests() {
            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();

            user.Id.Returns(1);
            user.Status.Returns(UserStatus.Active);
            user.Email.Returns("test@test.com");

            player.User.Returns(user);
            player.Buildings.Returns(new List<Building>() {
                new Building(player, BuildingType.Lumberyard, 2)
            });

            _context.Users.Add(user);
            _context.Players.Add(player);
        }

        [TestMethod]
        public void GetBuildingQueryHandler_Returns_Correct_Values_For_Existing_Building() {
            var handler = new GetBuildingQueryHandler(_context, _resourcesMap);
            var query = new GetBuildingQuery("test@test.com", "Lumberyard");

            var result = handler.Execute(query);

            result.Level.Should().Be(2);
            result.Name.Should().Be("Lumberyard (level 2)");
            result.UpdateCost.Gold.Should().Be(150000);
        }

        [TestMethod]
        public void GetBuildingQueryHandler_Returns_Correct_Values_For_New_Building() {
            var handler = new GetBuildingQueryHandler(_context, _resourcesMap);
            var query = new GetBuildingQuery("test@test.com", "Farm");

            var result = handler.Execute(query);

            result.Level.Should().Be(0);
            result.Name.Should().Be("Farm (level 0)");
            result.UpdateCost.Gold.Should().Be(20000);
        }

        [TestMethod]
        public void GetBuildingQueryHandler_Throws_Exception_For_Invalid_BuildingType() {
            var handler = new GetBuildingQueryHandler(_context, _resourcesMap);
            var query = new GetBuildingQuery("test@test.com", "Wrong");

            Action action = () => handler.Execute(query);

            action.Should().Throw<ArgumentException>();
        }
    }
}