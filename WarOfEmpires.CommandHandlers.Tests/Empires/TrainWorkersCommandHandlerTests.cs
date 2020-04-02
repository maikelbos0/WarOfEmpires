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
    public sealed class TrainWorkersCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _player;

        public TrainWorkersCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);

            var player = Substitute.For<Player>();
            player.User.Returns(user);
            player.Peasants.Returns(20);
            player.CanAfford(Arg.Any<Resources>()).Returns(true);
            player.GetAvailableHutCapacity().Returns(20);

            _context.Users.Add(user);
            _context.Players.Add(player);
            _player = player;
        }

        [TestMethod]
        public void TrainWorkersCommandHandler_Succeeds() {
            var handler = new TrainWorkersCommandHandler(_repository);
            var command = new TrainWorkersCommand("test@test.com", new List<WorkerInfo>() {
                new WorkerInfo("Farmers", "5"),
                new WorkerInfo("WoodWorkers", "4"),
                new WorkerInfo("StoneMasons", "3"),
                new WorkerInfo("OreMiners", "2"),
                new WorkerInfo("SiegeEngineers", "1"),
                new WorkerInfo("Merchants", "2")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.Received().TrainWorkers(WorkerType.Farmers, 5);
            _player.Received().TrainWorkers(WorkerType.WoodWorkers, 4);
            _player.Received().TrainWorkers(WorkerType.StoneMasons, 3);
            _player.Received().TrainWorkers(WorkerType.OreMiners, 2);
            _player.Received().TrainWorkers(WorkerType.SiegeEngineers, 1);
            _player.Received().TrainWorkers(WorkerType.Merchants, 2);
            _context.CallsToSaveChanges.Should().Be(1);
        }
        
        [TestMethod]
        public void TrainWorkersCommandHandler_Allows_Empty_Values() {
            var handler = new TrainWorkersCommandHandler(_repository);
            var command = new TrainWorkersCommand("test@test.com", new List<WorkerInfo>() {
                new WorkerInfo("Farmers", ""),
                new WorkerInfo("WoodWorkers", ""),
                new WorkerInfo("StoneMasons", ""),
                new WorkerInfo("OreMiners", ""),
                new WorkerInfo("SiegeEngineers", ""),
                new WorkerInfo("Merchants", "")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.DidNotReceiveWithAnyArgs().TrainWorkers(default, default);
        }
        
        [TestMethod]
        public void TrainWorkersCommandHandler_Fails_For_Too_Little_Huts_Room() {
            _player.Peasants.Returns(30);

            var handler = new TrainWorkersCommandHandler(_repository);
            var command = new TrainWorkersCommand("test@test.com", new List<WorkerInfo>() {
                new WorkerInfo("Farmers", "4"),
                new WorkerInfo("WoodWorkers", "4"),
                new WorkerInfo("StoneMasons", "4"),
                new WorkerInfo("OreMiners", "4"),
                new WorkerInfo("SiegeEngineers", "4"),
                new WorkerInfo("Merchants", "4")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have enough huts available to train that many workers");
            _player.DidNotReceiveWithAnyArgs().TrainWorkers(default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void TrainWorkersCommandHandler_Throws_Exception_For_Invalid_Type() {
            var handler = new TrainWorkersCommandHandler(_repository);
            var command = new TrainWorkersCommand("test@test.com", new List<WorkerInfo>() {
                new WorkerInfo("Test", "1")
            });

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();
            _player.DidNotReceiveWithAnyArgs().TrainWorkers(default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void TrainWorkersCommandHandler_Fails_For_Alphanumeric_Count() {
            var handler = new TrainWorkersCommandHandler(_repository);
            var command = new TrainWorkersCommand("test@test.com", new List<WorkerInfo>() {
                new WorkerInfo("Farmers", "A")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Workers[0].Count", "Invalid number");
            _player.DidNotReceiveWithAnyArgs().TrainWorkers(default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void TrainWorkersCommandHandler_Fails_For_Negative_Count() {
            var handler = new TrainWorkersCommandHandler(_repository);
            var command = new TrainWorkersCommand("test@test.com", new List<WorkerInfo>() {
                new WorkerInfo("Farmers", "-1")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Workers[0].Count", "Invalid number");
            _player.DidNotReceiveWithAnyArgs().TrainWorkers(default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void TrainWorkersCommandHandler_Fails_For_Too_High_Count() {
            _player.Peasants.Returns(15);

            var handler = new TrainWorkersCommandHandler(_repository);
            var command = new TrainWorkersCommand("test@test.com", new List<WorkerInfo>() {
                new WorkerInfo("Farmers", "3"),
                new WorkerInfo("WoodWorkers", "3"),
                new WorkerInfo("StoneMasons", "3"),
                new WorkerInfo("OreMiners", "3"),
                new WorkerInfo("SiegeEngineers", "2"),
                new WorkerInfo("Merchants", "2")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have that many peasants available to train");
            _player.DidNotReceiveWithAnyArgs().TrainWorkers(default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void TrainWorkersCommandHandler_Fails_For_Too_Little_Resources() {
            _player.CanAfford(Arg.Any<Resources>()).Returns(r => r.ArgAt<Resources>(0).Equals(new Resources(0)));

            var handler = new TrainWorkersCommandHandler(_repository);
            var command = new TrainWorkersCommand("test@test.com", new List<WorkerInfo>() {
                new WorkerInfo("Farmers", "1")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have enough gold to train these peasants");
            _player.DidNotReceiveWithAnyArgs().TrainWorkers(default, default);
            _context.CallsToSaveChanges.Should().Be(0);
        }
    }
}