using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Siege;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class DiscardSiegeCommandHandlerTests {
        [TestMethod]
        public void DiscardSiegeCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithSiege(SiegeWeaponType.FireArrows, 3)
                .WithSiege(SiegeWeaponType.BatteringRams, 3)
                .WithSiege(SiegeWeaponType.ScalingLadders, 3);

            var handler = new DiscardSiegeCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new DiscardSiegeCommand("test1@test.com", new List<SiegeWeaponInfo>() {
                new SiegeWeaponInfo("FireArrows", "1"),
                new SiegeWeaponInfo("BatteringRams", "2"),
                new SiegeWeaponInfo("ScalingLadders", "3")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.Received().DiscardSiege(SiegeWeaponType.FireArrows, 1);
            builder.Player.Received().DiscardSiege(SiegeWeaponType.BatteringRams, 2);
            builder.Player.Received().DiscardSiege(SiegeWeaponType.ScalingLadders, 3);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }
        
        [TestMethod]
        public void DiscardSiegeCommandHandler_Allows_Empty_Values() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new DiscardSiegeCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new DiscardSiegeCommand("test1@test.com", new List<SiegeWeaponInfo>() {
                new SiegeWeaponInfo("FireArrows", ""),
                new SiegeWeaponInfo("BatteringRams", ""),
                new SiegeWeaponInfo("ScalingLadders", "")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.DidNotReceiveWithAnyArgs().DiscardSiege(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void DiscardSiegeCommandHandler_Throws_Exception_For_Invalid_Type() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new DiscardSiegeCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new DiscardSiegeCommand("test1@test.com", new List<SiegeWeaponInfo>() {
                new SiegeWeaponInfo("Test", "1")
            });

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();
            builder.Player.DidNotReceiveWithAnyArgs().DiscardSiege(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void DiscardSiegeCommandHandler_Fails_For_Alphanumeric_Count() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new DiscardSiegeCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new DiscardSiegeCommand("test1@test.com", new List<SiegeWeaponInfo>() {
                new SiegeWeaponInfo("FireArrows", "A")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("SiegeWeapons[0].Count", "Invalid number");
            builder.Player.DidNotReceiveWithAnyArgs().DiscardSiege(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void DiscardSiegeCommandHandler_Fails_For_Negative_Count() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new DiscardSiegeCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new DiscardSiegeCommand("test1@test.com", new List<SiegeWeaponInfo>() {
                new SiegeWeaponInfo("FireArrows", "-1")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("SiegeWeapons[0].Count", "Invalid number");
            builder.Player.DidNotReceiveWithAnyArgs().DiscardSiege(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [DataTestMethod]
        [DataRow("FireArrows", "You don't have that many fire arrows to discard")]
        [DataRow("BatteringRams", "You don't have that many battering rams to discard")]
        [DataRow("ScalingLadders", "You don't have that many scaling ladders to discard")]
        public void DiscardSiegeCommandHandler_Fails_For_Too_High_Count(string type, string message) {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithSiege(SiegeWeaponType.FireArrows, 3)
                .WithSiege(SiegeWeaponType.BatteringRams, 3)
                .WithSiege(SiegeWeaponType.ScalingLadders, 3);

            var handler = new DiscardSiegeCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new DiscardSiegeCommand("test1@test.com", new List<SiegeWeaponInfo>() {
                new SiegeWeaponInfo(type, "4")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("SiegeWeapons[0].Count", message);
            builder.Player.DidNotReceiveWithAnyArgs().DiscardSiege(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}