using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class UntrainTroopsCommandHandlerTests {
        [TestMethod]
        public void UntrainTroopsCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithTroops(TroopType.Archers, 10, 10)
                .WithTroops(TroopType.Cavalry, 10, 10)
                .WithTroops(TroopType.Footmen, 10, 10);

            var handler = new UntrainTroopsCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new UntrainTroopsCommand("test1@test.com", new List<TroopInfo>(){
                new TroopInfo("Archers", "0", "1"),
                new TroopInfo("Cavalry", "2", "3"),
                new TroopInfo("Footmen", "4", "5")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.Received().UntrainTroops(TroopType.Archers, 0, 1);
            builder.Player.Received().UntrainTroops(TroopType.Cavalry, 2, 3);
            builder.Player.Received().UntrainTroops(TroopType.Footmen, 4, 5);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Allows_Empty_Troops() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithTroops(TroopType.Archers, 10, 10)
                .WithTroops(TroopType.Cavalry, 10, 10)
                .WithTroops(TroopType.Footmen, 10, 10);

            var handler = new UntrainTroopsCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new UntrainTroopsCommand("test1@test.com", new List<TroopInfo>(){
                new TroopInfo("Archers", "", ""),
                new TroopInfo("Cavalry", "", ""),
                new TroopInfo("Footmen", "", "")
            });

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Throws_Exception_For_Invalid_Type() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new UntrainTroopsCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new UntrainTroopsCommand("test1@test.com", new List<TroopInfo>() {
                new TroopInfo("Test", "1", "1")
            });

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();
            builder.Player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Alphanumeric_Soldiers() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithTroops(TroopType.Archers, 10, 10);

            var handler = new UntrainTroopsCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new UntrainTroopsCommand("test1@test.com", new List<TroopInfo>() {
                new TroopInfo("Archers", "A", "1")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Troops[0].Soldiers", "Invalid number");
            builder.Player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Alphanumeric_Mercenaries() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithTroops(TroopType.Archers, 10, 10);

            var handler = new UntrainTroopsCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new UntrainTroopsCommand("test1@test.com", new List<TroopInfo>() {
                new TroopInfo("Archers", "1", "A")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Troops[0].Mercenaries", "Invalid number");
            builder.Player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Negative_Soldiers() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithTroops(TroopType.Archers, 10, 10);

            var handler = new UntrainTroopsCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new UntrainTroopsCommand("test1@test.com", new List<TroopInfo>() {
                new TroopInfo("Archers", "-1", "1")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Troops[0].Soldiers", "Invalid number");
            builder.Player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Negative_Mercenaries() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithTroops(TroopType.Archers, 10, 10);

            var handler = new UntrainTroopsCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new UntrainTroopsCommand("test1@test.com", new List<TroopInfo>() {
                new TroopInfo("Archers", "1", "-1")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Troops[0].Mercenaries", "Invalid number");
            builder.Player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Too_High_Soldiers() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithTroops(TroopType.Archers, 10, 10);

            var handler = new UntrainTroopsCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new UntrainTroopsCommand("test1@test.com", new List<TroopInfo>() {
                new TroopInfo("Archers", "12", "2")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Troops[0].Soldiers", "You don't have that many archers to untrain");
            builder.Player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Too_High_Mercenaries() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithTroops(TroopType.Archers, 10, 10);

            var handler = new UntrainTroopsCommandHandler(new PlayerRepository(builder.Context), new EnumFormatter());
            var command = new UntrainTroopsCommand("test1@test.com", new List<TroopInfo>() {
                new TroopInfo("Archers", "2", "12")
            });

            var result = handler.Execute(command);

            result.Should().HaveError("Troops[0].Mercenaries", "You don't have that many mercenary archers to untrain");
            builder.Player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}