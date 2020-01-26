using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Domain.Siege;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class DiscardSiegeCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _player;

        public DiscardSiegeCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);

            var player = Substitute.For<Player>();
            player.User.Returns(user);
            player.SiegeWeapons.Returns(new List<SiegeWeapon>() {
                new SiegeWeapon(player, SiegeWeaponType.FireArrows, 3),
                new SiegeWeapon(player, SiegeWeaponType.BatteringRams, 3),
                new SiegeWeapon(player, SiegeWeaponType.ScalingLadders, 3)
            });

            _context.Users.Add(user);
            _context.Players.Add(player);
            _player = player;
        }

        [TestMethod]
        public void DiscardSiegeCommandHandler_Succeeds() {
            var handler = new DiscardSiegeCommandHandler(_repository);
            var command = new DiscardSiegeCommand("test@test.com", "1", "2", "3");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.Received().DiscardSiege(SiegeWeaponType.FireArrows, 1);
            _player.Received().DiscardSiege(SiegeWeaponType.BatteringRams, 2);
            _player.Received().DiscardSiege(SiegeWeaponType.ScalingLadders, 3);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void DiscardSiegeCommandHandler_Allows_Empty_Values() {
            var handler = new DiscardSiegeCommandHandler(_repository);
            var command = new DiscardSiegeCommand("test@test.com", "", "", "");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.DidNotReceiveWithAnyArgs().DiscardSiege(default, default);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void DiscardSiegeCommandHandler_Fails_For_Alphanumeric_FireArrows() {
            var handler = new DiscardSiegeCommandHandler(_repository);
            var command = new DiscardSiegeCommand("test@test.com", "A", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.FireArrows");
            result.Errors[0].Message.Should().Be("Fire arrows must be a valid number");
            _player.DidNotReceiveWithAnyArgs().DiscardSiege(default, default);
        }

        [TestMethod]
        public void DiscardSiegeCommandHandler_Fails_For_Alphanumeric_BatteringRams() {
            var handler = new DiscardSiegeCommandHandler(_repository);
            var command = new DiscardSiegeCommand("test@test.com", "2", "A", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.BatteringRams");
            result.Errors[0].Message.Should().Be("Battering rams must be a valid number");
            _player.DidNotReceiveWithAnyArgs().DiscardSiege(default, default);
        }

        [TestMethod]
        public void DiscardSiegeCommandHandler_Fails_For_Alphanumeric_ScalingLadders() {
            var handler = new DiscardSiegeCommandHandler(_repository);
            var command = new DiscardSiegeCommand("test@test.com", "2", "2", "A");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.ScalingLadders");
            result.Errors[0].Message.Should().Be("Scaling ladders must be a valid number");
            _player.DidNotReceiveWithAnyArgs().DiscardSiege(default, default);
        }

        [TestMethod]
        public void DiscardSiegeCommandHandler_Fails_For_Too_High_FireArrows() {
            var handler = new DiscardSiegeCommandHandler(_repository);
            var command = new DiscardSiegeCommand("test@test.com", "4", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.FireArrows");
            result.Errors[0].Message.Should().Be("You don't have that many fire arrows to discard");
            _player.DidNotReceiveWithAnyArgs().DiscardSiege(default, default);
        }

        [TestMethod]
        public void DiscardSiegeCommandHandler_Fails_For_Too_High_BatteringRams() {
            var handler = new DiscardSiegeCommandHandler(_repository);
            var command = new DiscardSiegeCommand("test@test.com", "2", "4", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.BatteringRams");
            result.Errors[0].Message.Should().Be("You don't have that many battering rams to discard");
            _player.DidNotReceiveWithAnyArgs().DiscardSiege(default, default);
        }

        [TestMethod]
        public void DiscardSiegeCommandHandler_Fails_For_Too_High_ScalingLadders() {
            var handler = new DiscardSiegeCommandHandler(_repository);
            var command = new DiscardSiegeCommand("test@test.com", "2", "2", "4");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.ScalingLadders");
            result.Errors[0].Message.Should().Be("You don't have that many scaling ladders to discard");
            _player.DidNotReceiveWithAnyArgs().DiscardSiege(default, default);
        }

        [TestMethod]
        public void DiscardSiegeCommandHandler_Fails_For_Negative_FireArrows() {
            var handler = new DiscardSiegeCommandHandler(_repository);
            var command = new DiscardSiegeCommand("test@test.com", "-1", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.FireArrows");
            result.Errors[0].Message.Should().Be("Fire arrows must be a valid number");
            _player.DidNotReceiveWithAnyArgs().DiscardSiege(default, default);
        }

        [TestMethod]
        public void DiscardSiegeCommandHandler_Fails_For_Negative_BatteringRams() {
            var handler = new DiscardSiegeCommandHandler(_repository);
            var command = new DiscardSiegeCommand("test@test.com", "2", "-1", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.BatteringRams");
            result.Errors[0].Message.Should().Be("Battering rams must be a valid number");
            _player.DidNotReceiveWithAnyArgs().DiscardSiege(default, default);
        }

        [TestMethod]
        public void DiscardSiegeCommandHandler_Fails_For_Negative_ScalingLadders() {
            var handler = new DiscardSiegeCommandHandler(_repository);
            var command = new DiscardSiegeCommand("test@test.com", "2", "2", "-1");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.ScalingLadders");
            result.Errors[0].Message.Should().Be("Scaling ladders must be a valid number");
            _player.DidNotReceiveWithAnyArgs().DiscardSiege(default, default);
        }
    }
}