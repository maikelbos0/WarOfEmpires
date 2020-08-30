using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;
using System;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class TrainTroopsCommandHandlerTests {
        [TestMethod]
        public void TrainTroopsCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithPeasants(20);

            builder.Player.GetAvailableBarracksCapacity().Returns(20);

            var handler = new TrainTroopsCommandHandler(new PlayerRepository(builder.Context));
            var command = new TrainTroopsCommand("test1@test.com", new List<TroopInfo>(){
                new TroopInfo("Archers", "6", "3"),
                new TroopInfo("Cavalry", "4", "2"),
                new TroopInfo("Footmen", "4", "1")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.Received().TrainTroops(TroopType.Archers, 6, 3);
            builder.Player.Received().TrainTroops(TroopType.Cavalry, 4, 2);
            builder.Player.Received().TrainTroops(TroopType.Footmen, 4, 1);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }
        
        [TestMethod]
        public void TrainTroopsCommandHandler_Allows_Empty_Troops() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithPeasants(30);

            var handler = new TrainTroopsCommandHandler(new PlayerRepository(builder.Context));
            var command = new TrainTroopsCommand("test1@test.com", new List<TroopInfo>(){
                new TroopInfo("Archers", "", ""),
                new TroopInfo("Cavalry", "", ""),
                new TroopInfo("Footmen", "", "")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Throws_Exception_For_Invalid_Type() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new TrainTroopsCommandHandler(new PlayerRepository(builder.Context));
            var command = new TrainTroopsCommand("test1@test.com", new List<TroopInfo>() {
                new TroopInfo("Test", "1", "1")
            });

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();
            builder.Player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Too_Little_Barracks_Room() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithPeasants(30);

            builder.Player.GetAvailableBarracksCapacity().Returns(20);

            var handler = new TrainTroopsCommandHandler(new PlayerRepository(builder.Context));
            var command = new TrainTroopsCommand("test1@test.com", new List<TroopInfo>(){
                new TroopInfo("Archers", "5", "2"),
                new TroopInfo("Cavalry", "5", "2"),
                new TroopInfo("Footmen", "5", "2")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have enough barracks available to train that many troops");
            builder.Player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Alphanumeric_Soldiers() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new TrainTroopsCommandHandler(new PlayerRepository(builder.Context));
            var command = new TrainTroopsCommand("test1@test.com", new List<TroopInfo>(){
                new TroopInfo("Archers", "A", "2")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Troops[0].Soldiers", "Invalid number");
            builder.Player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Alphanumeric_Mercenaries() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new TrainTroopsCommandHandler(new PlayerRepository(builder.Context));
            var command = new TrainTroopsCommand("test1@test.com", new List<TroopInfo>(){
                new TroopInfo("Archers", "2", "A")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Troops[0].Mercenaries", "Invalid number");
            builder.Player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Negative_Soldiers() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new TrainTroopsCommandHandler(new PlayerRepository(builder.Context));
            var command = new TrainTroopsCommand("test1@test.com", new List<TroopInfo>(){
                new TroopInfo("Archers", "-1", "2")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Troops[0].Soldiers", "Invalid number");
            builder.Player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Negative_Mercenaries() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new TrainTroopsCommandHandler(new PlayerRepository(builder.Context));
            var command = new TrainTroopsCommand("test1@test.com", new List<TroopInfo>(){
                new TroopInfo("Archers", "2", "-1")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Troops[0].Mercenaries", "Invalid number");
            builder.Player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
        
        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Too_High_TroopCounts() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithPeasants(30);

            builder.Player.GetAvailableBarracksCapacity().Returns(40);

            var handler = new TrainTroopsCommandHandler(new PlayerRepository(builder.Context));
            var command = new TrainTroopsCommand("test1@test.com", new List<TroopInfo>(){
                new TroopInfo("Archers", "11", "0"),
                new TroopInfo("Cavalry", "10", "0"),
                new TroopInfo("Footmen", "10", "0")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have that many peasants available to train");
            builder.Player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
        
        [TestMethod]
        public void TrainTroopsCommandHandler_Fails_For_Too_Little_Resources() {
            var builder = new FakeBuilder()
                .BuildPlayer(1, canAffordAnything: false)
                .WithPeasants(30);

            builder.Player.GetAvailableBarracksCapacity().Returns(20);

            var handler = new TrainTroopsCommandHandler(new PlayerRepository(builder.Context));
            var command = new TrainTroopsCommand("test1@test.com", new List<TroopInfo>(){
                new TroopInfo("Archers", "1", "1")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("You don't have enough resources to train these troops");
            builder.Player.DidNotReceiveWithAnyArgs().TrainTroops(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}