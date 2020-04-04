using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Domain.Siege;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class DiscardSiegeCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _player;
        private readonly EnumFormatter _formatter = new EnumFormatter();

        public DiscardSiegeCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);

            var player = Substitute.For<Player>();
            player.User.Returns(user);
            player.SiegeWeapons.Returns(new List<SiegeWeapon>() {
                new SiegeWeapon(SiegeWeaponType.FireArrows, 3),
                new SiegeWeapon(SiegeWeaponType.BatteringRams, 3),
                new SiegeWeapon(SiegeWeaponType.ScalingLadders, 3)
            });

            _context.Users.Add(user);
            _context.Players.Add(player);
            _player = player;
        }
        
        [TestMethod]
        public void DiscardSiegeCommandHandler_Succeeds() {
            var handler = new DiscardSiegeCommandHandler(_repository, _formatter);
            var command = new DiscardSiegeCommand("test@test.com", new List<SiegeWeaponInfo>() {
                new SiegeWeaponInfo("FireArrows", "1"),
                new SiegeWeaponInfo("BatteringRams", "2"),
                new SiegeWeaponInfo("ScalingLadders", "3")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.Received().DiscardSiege(SiegeWeaponType.FireArrows, 1);
            _player.Received().DiscardSiege(SiegeWeaponType.BatteringRams, 2);
            _player.Received().DiscardSiege(SiegeWeaponType.ScalingLadders, 3);
            _context.CallsToSaveChanges.Should().Be(1);
        }
        
        [TestMethod]
        public void DiscardSiegeCommandHandler_Allows_Empty_Values() {
            var handler = new DiscardSiegeCommandHandler(_repository, _formatter);
            var command = new DiscardSiegeCommand("test@test.com", new List<SiegeWeaponInfo>() {
                new SiegeWeaponInfo("FireArrows", ""),
                new SiegeWeaponInfo("BatteringRams", ""),
                new SiegeWeaponInfo("ScalingLadders", "")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.DidNotReceiveWithAnyArgs().DiscardSiege(default, default);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void DiscardSiegeCommandHandler_Throws_Exception_For_Invalid_Type() {
            var handler = new DiscardSiegeCommandHandler(_repository, _formatter);
            var command = new DiscardSiegeCommand("test@test.com", new List<SiegeWeaponInfo>() {
                new SiegeWeaponInfo("Test", "1")
            });

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();
            _player.DidNotReceiveWithAnyArgs().DiscardSiege(default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void DiscardSiegeCommandHandler_Fails_For_Alphanumeric_Count() {
            var handler = new DiscardSiegeCommandHandler(_repository, _formatter);
            var command = new DiscardSiegeCommand("test@test.com", new List<SiegeWeaponInfo>() {
                new SiegeWeaponInfo("FireArrows", "A")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("SiegeWeapons[0].Count", "Invalid number");
            _player.DidNotReceiveWithAnyArgs().DiscardSiege(default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void DiscardSiegeCommandHandler_Fails_For_Negative_Count() {
            var handler = new DiscardSiegeCommandHandler(_repository, _formatter);
            var command = new DiscardSiegeCommand("test@test.com", new List<SiegeWeaponInfo>() {
                new SiegeWeaponInfo("FireArrows", "-1")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("SiegeWeapons[0].Count", "Invalid number");
            _player.DidNotReceiveWithAnyArgs().DiscardSiege(default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [DataTestMethod]
        [DataRow("FireArrows", "You don't have that many fire arrows to discard")]
        [DataRow("BatteringRams", "You don't have that many battering rams to discard")]
        [DataRow("ScalingLadders", "You don't have that many scaling ladders to discard")]
        public void DiscardSiegeCommandHandler_Fails_For_Too_High_Count(string type, string message) {
            var handler = new DiscardSiegeCommandHandler(_repository, _formatter);
            var command = new DiscardSiegeCommand("test@test.com", new List<SiegeWeaponInfo>() {
                new SiegeWeaponInfo(type, "4")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("SiegeWeapons[0].Count", message);
            _player.DidNotReceiveWithAnyArgs().DiscardSiege(default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }
    }
}