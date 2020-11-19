using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class TrainWorkersCommandHandlerTests {
        [TestMethod]
        public void TrainWorkersCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithPeasants(20);

            builder.Player.GetAvailableHutCapacity().Returns(20);

            var handler = new TrainWorkersCommandHandler(new PlayerRepository(builder.Context));
            var command = new TrainWorkersCommand("test1@test.com", new List<WorkerInfo>() {
                new WorkerInfo("Farmers", 5),
                new WorkerInfo("WoodWorkers", 4),
                new WorkerInfo("StoneMasons", 3),
                new WorkerInfo("OreMiners", 2),
                new WorkerInfo("SiegeEngineers", 1),
                new WorkerInfo("Merchants", 2)
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.Received().TrainWorkers(WorkerType.Farmers, 5);
            builder.Player.Received().TrainWorkers(WorkerType.WoodWorkers, 4);
            builder.Player.Received().TrainWorkers(WorkerType.StoneMasons, 3);
            builder.Player.Received().TrainWorkers(WorkerType.OreMiners, 2);
            builder.Player.Received().TrainWorkers(WorkerType.SiegeEngineers, 1);
            builder.Player.Received().TrainWorkers(WorkerType.Merchants, 2);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }
        
        [TestMethod]
        public void TrainWorkersCommandHandler_Allows_Empty_Values() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new TrainWorkersCommandHandler(new PlayerRepository(builder.Context));
            var command = new TrainWorkersCommand("test1@test.com", new List<WorkerInfo>() {
                new WorkerInfo("Farmers", null),
                new WorkerInfo("WoodWorkers", null),
                new WorkerInfo("StoneMasons", null),
                new WorkerInfo("OreMiners", null),
                new WorkerInfo("SiegeEngineers", null),
                new WorkerInfo("Merchants", null)
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.DidNotReceiveWithAnyArgs().TrainWorkers(default, default);
        }
        
        [TestMethod]
        public void TrainWorkersCommandHandler_Fails_For_Too_Little_Huts_Room() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithPeasants(30);

            builder.Player.GetAvailableHutCapacity().Returns(20);

            var handler = new TrainWorkersCommandHandler(new PlayerRepository(builder.Context));
            var command = new TrainWorkersCommand("test1@test.com", new List<WorkerInfo>() {
                new WorkerInfo("Farmers", 4),
                new WorkerInfo("WoodWorkers", 4),
                new WorkerInfo("StoneMasons", 4),
                new WorkerInfo("OreMiners", 4),
                new WorkerInfo("SiegeEngineers", 4),
                new WorkerInfo("Merchants", 4)
            });

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have enough huts available to train that many workers");
            builder.Player.DidNotReceiveWithAnyArgs().TrainWorkers(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void TrainWorkersCommandHandler_Throws_Exception_For_Invalid_Type() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new TrainWorkersCommandHandler(new PlayerRepository(builder.Context));
            var command = new TrainWorkersCommand("test1@test.com", new List<WorkerInfo>() {
                new WorkerInfo("Test", 1)
            });

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();
            builder.Player.DidNotReceiveWithAnyArgs().TrainWorkers(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void TrainWorkersCommandHandler_Fails_For_Too_High_Count() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithPeasants(15);

            builder.Player.GetAvailableHutCapacity().Returns(20);

            var handler = new TrainWorkersCommandHandler(new PlayerRepository(builder.Context));
            var command = new TrainWorkersCommand("test1@test.com", new List<WorkerInfo>() {
                new WorkerInfo("Farmers", 3),
                new WorkerInfo("WoodWorkers", 3),
                new WorkerInfo("StoneMasons", 3),
                new WorkerInfo("OreMiners", 3),
                new WorkerInfo("SiegeEngineers", 2),
                new WorkerInfo("Merchants", 2)
            });

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have that many peasants available to train");
            builder.Player.DidNotReceiveWithAnyArgs().TrainWorkers(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void TrainWorkersCommandHandler_Fails_For_Too_Little_Resources() {
            var builder = new FakeBuilder()
                .BuildPlayer(1, canAffordAnything: false)
                .WithPeasants(20);

            builder.Player.GetAvailableHutCapacity().Returns(20);

            var handler = new TrainWorkersCommandHandler(new PlayerRepository(builder.Context));
            var command = new TrainWorkersCommand("test1@test.com", new List<WorkerInfo>() {
                new WorkerInfo("Farmers", 1)
            });

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have enough gold to train these peasants");
            builder.Player.DidNotReceiveWithAnyArgs().TrainWorkers(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}