using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Siege;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class BuildSiegeCommandHandlerTests {
        [TestMethod]
        public void BuildSiegeCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithBuilding(BuildingType.SiegeFactory, 6)
                .WithWorkers(WorkerType.SiegeEngineers, 20);

            var handler = new BuildSiegeCommandHandler(new PlayerRepository(builder.Context));
            var command = new BuildSiegeCommand("test1@test.com", new List<SiegeWeaponInfo>() {
                new SiegeWeaponInfo("FireArrows", 1),
                new SiegeWeaponInfo("BatteringRams", 2),
                new SiegeWeaponInfo("ScalingLadders", 3)
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.Received().BuildSiege(SiegeWeaponType.FireArrows, 1);
            builder.Player.Received().BuildSiege(SiegeWeaponType.BatteringRams, 2);
            builder.Player.Received().BuildSiege(SiegeWeaponType.ScalingLadders, 3);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void BuildSiegeCommandHandler_Allows_Empty_Values() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithBuilding(BuildingType.SiegeFactory, 6)
                .WithWorkers(WorkerType.SiegeEngineers, 20);

            var handler = new BuildSiegeCommandHandler(new PlayerRepository(builder.Context));
            var command = new BuildSiegeCommand("test1@test.com", new List<SiegeWeaponInfo>() {
                new SiegeWeaponInfo("FireArrows", null),
                new SiegeWeaponInfo("BatteringRams", null),
                new SiegeWeaponInfo("ScalingLadders", null)
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.DidNotReceiveWithAnyArgs().BuildSiege(default, default);
        }

        [TestMethod]
        public void BuildSiegeCommandHandler_Fails_For_Too_Little_Maintenance() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new BuildSiegeCommandHandler(new PlayerRepository(builder.Context));
            var command = new BuildSiegeCommand("test1@test.com", new List<SiegeWeaponInfo>() {
                new SiegeWeaponInfo("FireArrows", 1)
            });

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have enough siege maintenance available to build that much siege");
            builder.Player.DidNotReceiveWithAnyArgs().BuildSiege(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuildSiegeCommandHandler_Throws_Exception_For_Invalid_Type() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new BuildSiegeCommandHandler(new PlayerRepository(builder.Context));
            var command = new BuildSiegeCommand("test1@test.com", new List<SiegeWeaponInfo>() {
                new SiegeWeaponInfo("Test", 1)
            });

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();
            builder.Player.DidNotReceiveWithAnyArgs().BuildSiege(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
        
        [TestMethod]
        public void BuildSiegeCommandHandler_Fails_For_Too_Little_Resources() {
            var builder = new FakeBuilder()
                .BuildPlayer(1, canAffordAnything: false)
                .WithBuilding(BuildingType.SiegeFactory, 6)
                .WithWorkers(WorkerType.SiegeEngineers, 20);

            var handler = new BuildSiegeCommandHandler(new PlayerRepository(builder.Context));
            var command = new BuildSiegeCommand("test1@test.com", new List<SiegeWeaponInfo>() {
                new SiegeWeaponInfo("FireArrows", 1)
            });

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have enough resources to build that much siege");
            builder.Player.DidNotReceiveWithAnyArgs().BuildSiege(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}