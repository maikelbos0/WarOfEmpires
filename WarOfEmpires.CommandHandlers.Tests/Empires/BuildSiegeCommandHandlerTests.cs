using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Domain.Siege;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class BuildSiegeCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _player;
        private readonly EnumFormatter _formatter = new EnumFormatter();

        public BuildSiegeCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);

            var player = Substitute.For<Player>();
            player.User.Returns(user);
            player.Workers.Returns(new List<Workers>() { new Workers(WorkerType.SiegeEngineers, 20) });
            player.GetBuildingBonus(BuildingType.SiegeFactory).Returns(6);
            player.CanAfford(Arg.Any<Resources>()).Returns(true);

            _context.Users.Add(user);
            _context.Players.Add(player);
            _player = player;
        }

        [TestMethod]
        public void BuildSiegeCommandHandler_Succceeds() {
            var handler = new BuildSiegeCommandHandler(_repository, _formatter);
            var command = new BuildSiegeCommand("test@test.com", "1", "2", "3");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.Received().BuildSiege(SiegeWeaponType.FireArrows, 1);
            _player.Received().BuildSiege(SiegeWeaponType.BatteringRams, 2);
            _player.Received().BuildSiege(SiegeWeaponType.ScalingLadders, 3);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void BuildSiegeCommandHandler_Allows_Empty_Values() {
            var handler = new BuildSiegeCommandHandler(_repository, _formatter);
            var command = new BuildSiegeCommand("test@test.com", "", "", "");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.DidNotReceiveWithAnyArgs().BuildSiege(default, default);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [DataTestMethod]
        [DataRow(1, 0, 0, DisplayName = "FireArrows")]
        [DataRow(0, 1, 0, DisplayName = "BatteringRams")]
        [DataRow(0, 0, 1, DisplayName = "ScalingLadders")]
        public void BuildSiegeCommandHandler_Fails_For_Too_Little_Maintenance(int fireArrows, int batteringRams, int scalingLadders) {
            _player.Workers.Returns(new List<Workers>() { new Workers(WorkerType.SiegeEngineers, 0) });

            var handler = new BuildSiegeCommandHandler(_repository, _formatter);
            var command = new BuildSiegeCommand("test@test.com", fireArrows.ToString(), batteringRams.ToString(), scalingLadders.ToString());

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.Should().BeNull();
            result.Errors[0].Message.Should().Be("You don't have enough siege maintenance available to build that much siege");
            _player.DidNotReceiveWithAnyArgs().BuildSiege(default, default);
        }

        [TestMethod]
        public void BuildSiegeCommandHandler_Fails_For_Alphanumeric_FireArrows() {
            var handler = new BuildSiegeCommandHandler(_repository, _formatter);
            var command = new BuildSiegeCommand("test@test.com", "A", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.FireArrows");
            result.Errors[0].Message.Should().Be("Fire arrows must be a valid number");
            _player.DidNotReceiveWithAnyArgs().BuildSiege(default, default);
        }

        [TestMethod]
        public void BuildSiegeCommandHandler_Fails_For_Alphanumeric_BatteringRams() {
            var handler = new BuildSiegeCommandHandler(_repository, _formatter);
            var command = new BuildSiegeCommand("test@test.com", "2", "A", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.BatteringRams");
            result.Errors[0].Message.Should().Be("Battering rams must be a valid number");
            _player.DidNotReceiveWithAnyArgs().BuildSiege(default, default);
        }

        [TestMethod]
        public void BuildSiegeCommandHandler_Fails_For_Alphanumeric_ScalingLadders() {
            var handler = new BuildSiegeCommandHandler(_repository, _formatter);
            var command = new BuildSiegeCommand("test@test.com", "2", "2", "A");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.ScalingLadders");
            result.Errors[0].Message.Should().Be("Scaling ladders must be a valid number");
            _player.DidNotReceiveWithAnyArgs().BuildSiege(default, default);
        }

        [TestMethod]
        public void BuildSiegeCommandHandler_Fails_For_Negative_FireArrows() {
            var handler = new BuildSiegeCommandHandler(_repository, _formatter);
            var command = new BuildSiegeCommand("test@test.com", "-1", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.FireArrows");
            result.Errors[0].Message.Should().Be("Fire arrows must be a valid number");
            _player.DidNotReceiveWithAnyArgs().BuildSiege(default, default);
        }

        [TestMethod]
        public void BuildSiegeCommandHandler_Fails_For_Negative_BatteringRams() {
            var handler = new BuildSiegeCommandHandler(_repository, _formatter);
            var command = new BuildSiegeCommand("test@test.com", "2", "-1", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.BatteringRams");
            result.Errors[0].Message.Should().Be("Battering rams must be a valid number");
            _player.DidNotReceiveWithAnyArgs().BuildSiege(default, default);
        }

        [TestMethod]
        public void BuildSiegeCommandHandler_Fails_For_Negative_ScalingLadders() {
            var handler = new BuildSiegeCommandHandler(_repository, _formatter);
            var command = new BuildSiegeCommand("test@test.com", "2", "2", "-1");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.ScalingLadders");
            result.Errors[0].Message.Should().Be("Scaling ladders must be a valid number");
            _player.DidNotReceiveWithAnyArgs().BuildSiege(default, default);
        }

        [DataTestMethod]
        [DataRow(1, 0, 0, DisplayName = "FireArrows")]
        [DataRow(0, 1, 0, DisplayName = "BatteringRams")]
        [DataRow(0, 0, 1, DisplayName = "ScalingLadders")]
        public void BuildSiegeCommandHandler_Fails_For_Too_Little_Resources(int fireArrows, int batteringRams, int scalingLadders) {
            _player.CanAfford(Arg.Any<Resources>()).Returns(r => r.ArgAt<Resources>(0).Equals(new Resources(0)));

            var handler = new BuildSiegeCommandHandler(_repository, _formatter);
            var command = new BuildSiegeCommand("test@test.com", fireArrows.ToString(), batteringRams.ToString(), scalingLadders.ToString());

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.Should().BeNull();
            result.Errors[0].Message.Should().Be("You don't have enough resources to build that much siege");
            _player.DidNotReceiveWithAnyArgs().BuildSiege(default, default);
        }
    }
}