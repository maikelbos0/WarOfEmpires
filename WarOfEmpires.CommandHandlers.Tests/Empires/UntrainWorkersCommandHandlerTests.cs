using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Domain.Siege;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class UntrainWorkersCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _player;
        private readonly EnumFormatter _formatter = new EnumFormatter();

        public UntrainWorkersCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);

            var player = Substitute.For<Player>();
            player.User.Returns(user);
            player.Workers.Returns(new List<Workers>() {
                new Workers(WorkerType.Farmers, 10),
                new Workers(WorkerType.WoodWorkers, 10),
                new Workers(WorkerType.StoneMasons, 10),
                new Workers(WorkerType.OreMiners, 10),
                new Workers(WorkerType.SiegeEngineers, 10),
                new Workers(WorkerType.Merchants, 10),
            });

            _context.Users.Add(user);
            _context.Players.Add(player);
            _player = player;
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Succeeds() {
            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", new List<WorkerInfo>() {
                new WorkerInfo("Farmers", "5"),
                new WorkerInfo("WoodWorkers", "4"),
                new WorkerInfo("StoneMasons", "3"),
                new WorkerInfo("OreMiners", "2"),
                new WorkerInfo("SiegeEngineers", "1"),
                new WorkerInfo("Merchants", "6")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.Received().UntrainWorkers(WorkerType.Farmers, 5);
            _player.Received().UntrainWorkers(WorkerType.WoodWorkers, 4);
            _player.Received().UntrainWorkers(WorkerType.StoneMasons, 3);
            _player.Received().UntrainWorkers(WorkerType.OreMiners, 2);
            _player.Received().UntrainWorkers(WorkerType.SiegeEngineers, 1);
            _player.Received().UntrainWorkers(WorkerType.Merchants, 6);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Allows_Empty_Workers() {
            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", new List<WorkerInfo>() {
                new WorkerInfo("Farmers", ""),
                new WorkerInfo("WoodWorkers", ""),
                new WorkerInfo("StoneMasons", ""),
                new WorkerInfo("OreMiners", ""),
                new WorkerInfo("SiegeEngineers", ""),
                new WorkerInfo("Merchants", "")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Throws_Exception_For_Invalid_Type() {
            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", new List<WorkerInfo>() {
                new WorkerInfo("Test", "1")
            });

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();
            _player.DidNotReceiveWithAnyArgs().TrainWorkers(default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Alphanumeric_Count() {
            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", new List<WorkerInfo>() {
                new WorkerInfo("Farmers", "A")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Workers[0].Count", "Invalid number");
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Negative_Count() {
            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", new List<WorkerInfo>() {
                new WorkerInfo("Farmers", "-1")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Workers[0].Count", "Invalid number");
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Too_High_Count() {
            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", new List<WorkerInfo>() {
                new WorkerInfo("Farmers", "12")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Workers[0].Count", "You don't have that many farmers to untrain");
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Siege_Maintenance_In_Use() {
            _player.GetBuildingBonus(BuildingType.SiegeFactory).Returns(6);
            _player.SiegeWeapons.Returns(new List<SiegeWeapon>() {
                new SiegeWeapon(SiegeWeaponType.FireArrows, 2),
                new SiegeWeapon(SiegeWeaponType.BatteringRams, 2),
                new SiegeWeapon(SiegeWeaponType.ScalingLadders, 2)
            });

            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", new List<WorkerInfo>() {
                new WorkerInfo("SiegeEngineers", "1")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Workers[0].Count", "Your siege engineers are maintaining too many siege weapons for that many to be untrained");
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Merchants_In_Use() {
            _player.Caravans.Returns(new List<Caravan>() {
                new Caravan(_player),
                new Caravan(_player)
            });

            var handler = new UntrainWorkersCommandHandler(_repository, _formatter);
            var command = new UntrainWorkersCommand("test@test.com", new List<WorkerInfo>() {
                new WorkerInfo("Merchants", "9")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Workers[0].Count", "You can not untrain merchants that have a caravan on the market");
            _player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }
    }
}