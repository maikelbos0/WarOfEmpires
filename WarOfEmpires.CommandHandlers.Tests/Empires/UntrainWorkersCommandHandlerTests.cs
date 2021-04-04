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
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class UntrainWorkersCommandHandlerTests {
        [TestMethod]
        public void UntrainWorkersCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithWorkers(WorkerType.Farmers, 10)
                .WithWorkers(WorkerType.WoodWorkers, 10)
                .WithWorkers(WorkerType.StoneMasons, 10)
                .WithWorkers(WorkerType.OreMiners, 10)
                .WithWorkers(WorkerType.SiegeEngineers, 10)
                .WithWorkers(WorkerType.Merchants, 10);

            var handler = new UntrainWorkersCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new UntrainWorkersCommand("test1@test.com", new List<WorkerInfo>() {
                new WorkerInfo("Farmers", 5),
                new WorkerInfo("WoodWorkers", 4),
                new WorkerInfo("StoneMasons", 3),
                new WorkerInfo("OreMiners", 2),
                new WorkerInfo("SiegeEngineers", 1),
                new WorkerInfo("Merchants", 6)
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.Received().UntrainWorkers(WorkerType.Farmers, 5);
            builder.Player.Received().UntrainWorkers(WorkerType.WoodWorkers, 4);
            builder.Player.Received().UntrainWorkers(WorkerType.StoneMasons, 3);
            builder.Player.Received().UntrainWorkers(WorkerType.OreMiners, 2);
            builder.Player.Received().UntrainWorkers(WorkerType.SiegeEngineers, 1);
            builder.Player.Received().UntrainWorkers(WorkerType.Merchants, 6);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Allows_Empty_Workers() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithWorkers(WorkerType.Farmers, 10)
                .WithWorkers(WorkerType.WoodWorkers, 10)
                .WithWorkers(WorkerType.StoneMasons, 10)
                .WithWorkers(WorkerType.OreMiners, 10)
                .WithWorkers(WorkerType.SiegeEngineers, 10)
                .WithWorkers(WorkerType.Merchants, 10);

            var handler = new UntrainWorkersCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new UntrainWorkersCommand("test1@test.com", new List<WorkerInfo>() {
                new WorkerInfo("Farmers", null),
                new WorkerInfo("WoodWorkers", null),
                new WorkerInfo("StoneMasons", null),
                new WorkerInfo("OreMiners", null),
                new WorkerInfo("SiegeEngineers", null),
                new WorkerInfo("Merchants", null)
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Throws_Exception_For_Invalid_Type() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithWorkers(WorkerType.WoodWorkers, 10);

            var handler = new UntrainWorkersCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new UntrainWorkersCommand("test1@test.com", new List<WorkerInfo>() {
                new WorkerInfo("Test", 1)
            });

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();
            builder.Player.DidNotReceiveWithAnyArgs().TrainWorkers(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Too_High_Count() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithWorkers(WorkerType.Farmers, 10);

            var handler = new UntrainWorkersCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new UntrainWorkersCommand("test1@test.com", new List<WorkerInfo>() {
                new WorkerInfo("Farmers", 11)
            });

            var result = handler.Execute(command);

            result.Should().HaveError(c => c.Workers[0].Count, "You don't have that many farmers to untrain");
            builder.Player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Siege_Maintenance_In_Use() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithWorkers(WorkerType.SiegeEngineers, 10)
                .WithBuilding(BuildingType.SiegeFactory, 6)
                .WithSiege(SiegeWeaponType.FireArrows, 2)
                .WithSiege(SiegeWeaponType.BatteringRams, 2)
                .WithSiege(SiegeWeaponType.ScalingLadders, 2);

            var handler = new UntrainWorkersCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new UntrainWorkersCommand("test1@test.com", new List<WorkerInfo>() {
                new WorkerInfo("SiegeEngineers", 1)
            });

            var result = handler.Execute(command);

            result.Should().HaveError(c => c.Workers[0].Count, "Your siege engineers are maintaining too many siege weapons for that many to be untrained");
            builder.Player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void UntrainWorkersCommandHandler_Fails_For_Merchants_In_Use() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithWorkers(WorkerType.Merchants, 10)
                .WithCaravan(1)
                .WithCaravan(1);

            var handler = new UntrainWorkersCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new UntrainWorkersCommand("test1@test.com", new List<WorkerInfo>() {
                new WorkerInfo("Merchants", 9)
            });

            var result = handler.Execute(command);

            result.Should().HaveError(c => c.Workers[0].Count, "You can't untrain merchants that have a caravan on the market");
            builder.Player.DidNotReceiveWithAnyArgs().UntrainWorkers(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}