using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class UpgradeBuildingCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _player;

        public UpgradeBuildingCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);

            var player = Substitute.For<Player>();
            player.User.Returns(user);
            player.CanAfford(Arg.Any<Resources>()).Returns(true);
            player.Buildings.Returns(new List<Building>() {
                new Building(player, BuildingType.Farm, 2),
                new Building(player, BuildingType.Lumberyard, 1)
            });

            _context.Users.Add(user);
            _context.Players.Add(player);
            _player = player;
        }

        [TestMethod]
        public void UpgradeBuildingCommandHandler_Succeeds_For_New_Building() {
            var handler = new UpgradeBuildingCommandHandler(_repository);
            var command = new UpgradeBuildingCommand("test@test.com", "Quarry");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.Received().UpgradeBuilding(BuildingType.Quarry);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void UpgradeBuildingCommandHandler_Succeeds_For_Existing_Building() {
            var handler = new UpgradeBuildingCommandHandler(_repository);
            var command = new UpgradeBuildingCommand("test@test.com", "Lumberyard");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.Received().UpgradeBuilding(BuildingType.Lumberyard);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void UpgradeBuildingCommandHandler_Throws_Exception_For_Invalid_BuildingType() {
            var handler = new UpgradeBuildingCommandHandler(_repository);
            var command = new UpgradeBuildingCommand("test@test.com", "Wrong");

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void UpgradeBuildingCommandHandler_Fails_For_Too_Little_Resources() {
            _player.CanAfford(Arg.Any<Resources>()).Returns(false);

            var handler = new UpgradeBuildingCommandHandler(_repository);
            var command = new UpgradeBuildingCommand("test@test.com", "Farm");

            var result = handler.Execute(command);
            
            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.Should().BeNull();
            result.Errors[0].Message.Should().Be("You don't have enough resources to upgrade your Farm (level 2)");
            _player.DidNotReceive().UpgradeBuilding(BuildingType.Farm);
        }
    }
}