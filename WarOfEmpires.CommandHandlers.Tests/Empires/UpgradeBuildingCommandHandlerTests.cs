using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class UpgradeBuildingCommandHandlerTests {
        [TestMethod]
        public void UpgradeBuildingCommandHandler_Succeeds_For_New_Building() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new UpgradeBuildingCommandHandler(new PlayerRepository(builder.Context));
            var command = new UpgradeBuildingCommand("test1@test.com", "Quarry");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.Received().UpgradeBuilding(BuildingType.Quarry);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void UpgradeBuildingCommandHandler_Succeeds_For_Existing_Building() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithBuilding(BuildingType.Lumberyard, 1);

            var handler = new UpgradeBuildingCommandHandler(new PlayerRepository(builder.Context));
            var command = new UpgradeBuildingCommand("test1@test.com", "Lumberyard");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.Received().UpgradeBuilding(BuildingType.Lumberyard);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void UpgradeBuildingCommandHandler_Throws_Exception_For_Invalid_BuildingType() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new UpgradeBuildingCommandHandler(new PlayerRepository(builder.Context));
            var command = new UpgradeBuildingCommand("test1@test.com", "Wrong");

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void UpgradeBuildingCommandHandler_Fails_For_Too_Little_Resources() {
            var builder = new FakeBuilder()
                .BuildPlayer(1, canAffordAnything: false)
                .WithBuilding(BuildingType.Farm, 2);

            var handler = new UpgradeBuildingCommandHandler(new PlayerRepository(builder.Context));
            var command = new UpgradeBuildingCommand("test1@test.com", "Farm");

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have enough resources to upgrade your Farm (level 2)");
            builder.Player.DidNotReceive().UpgradeBuilding(BuildingType.Farm);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}