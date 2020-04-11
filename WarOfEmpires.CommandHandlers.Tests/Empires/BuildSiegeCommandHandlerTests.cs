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
using WarOfEmpires.Domain.Siege;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class BuildSiegeCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _player;

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
        public void BuildSiegeCommandHandler_Succeeds() {
            var handler = new BuildSiegeCommandHandler(_repository);
            var command = new BuildSiegeCommand("test@test.com", new List<SiegeWeaponInfo>() {
                new SiegeWeaponInfo("FireArrows", "1"),
                new SiegeWeaponInfo("BatteringRams", "2"),
                new SiegeWeaponInfo("ScalingLadders", "3")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.Received().BuildSiege(SiegeWeaponType.FireArrows, 1);
            _player.Received().BuildSiege(SiegeWeaponType.BatteringRams, 2);
            _player.Received().BuildSiege(SiegeWeaponType.ScalingLadders, 3);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void BuildSiegeCommandHandler_Allows_Empty_Values() {
            var handler = new BuildSiegeCommandHandler(_repository);
            var command = new BuildSiegeCommand("test@test.com", new List<SiegeWeaponInfo>() {
                new SiegeWeaponInfo("FireArrows", ""),
                new SiegeWeaponInfo("BatteringRams", ""),
                new SiegeWeaponInfo("ScalingLadders", "")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.DidNotReceiveWithAnyArgs().BuildSiege(default, default);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void BuildSiegeCommandHandler_Fails_For_Too_Little_Maintenance() {
            _player.Workers.Returns(new List<Workers>() { new Workers(WorkerType.SiegeEngineers, 0) });

            var handler = new BuildSiegeCommandHandler(_repository);
            var command = new BuildSiegeCommand("test@test.com", new List<SiegeWeaponInfo>() {
                new SiegeWeaponInfo("FireArrows", "1")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have enough siege maintenance available to build that much siege");
            _player.DidNotReceiveWithAnyArgs().BuildSiege(default, default);
        }

        [TestMethod]
        public void BuildSiegeCommandHandler_Throws_Exception_For_Invalid_Type() {
            var handler = new BuildSiegeCommandHandler(_repository);
            var command = new BuildSiegeCommand("test@test.com", new List<SiegeWeaponInfo>() {
                new SiegeWeaponInfo("Test", "1")
            });

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();
            _player.DidNotReceiveWithAnyArgs().BuildSiege(default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuildSiegeCommandHandler_Fails_For_Alphanumeric_Count() {
            var handler = new BuildSiegeCommandHandler(_repository);
            var command = new BuildSiegeCommand("test@test.com", new List<SiegeWeaponInfo>() {
                new SiegeWeaponInfo("FireArrows", "A")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("SiegeWeapons[0].Count", "Invalid number");
            _player.DidNotReceiveWithAnyArgs().BuildSiege(default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void BuildSiegeCommandHandler_Fails_For_Negative_Count() {
            var handler = new BuildSiegeCommandHandler(_repository);
            var command = new BuildSiegeCommand("test@test.com", new List<SiegeWeaponInfo>() {
                new SiegeWeaponInfo("FireArrows", "-1")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("SiegeWeapons[0].Count", "Invalid number");
            _player.DidNotReceiveWithAnyArgs().BuildSiege(default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }
        
        [TestMethod]
        public void BuildSiegeCommandHandler_Fails_For_Too_Little_Resources() {
            _player.CanAfford(Arg.Any<Resources>()).Returns(r => r.ArgAt<Resources>(0).Equals(new Resources(0)));

            var handler = new BuildSiegeCommandHandler(_repository);
            var command = new BuildSiegeCommand("test@test.com", new List<SiegeWeaponInfo>() {
                new SiegeWeaponInfo("FireArrows", "1")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have enough resources to build that much siege");
            _player.DidNotReceiveWithAnyArgs().BuildSiege(default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }
    }
}