using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.CommandHandlers.Attacks;
using WarOfEmpires.Commands.Attacks;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Attacks {
    [TestClass]
    public sealed class AttackCommandHandlerTests {
        [TestMethod]
        public void AttackCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var attacker)
                .WithPlayer(2, out var defender);

            var handler = new AttackCommandHandler(new PlayerRepository(builder.Context));
            var command = new AttackCommand("Raid", "test1@test.com", "2", "10");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            attacker.Received().ExecuteAttack(AttackType.Raid, defender, 10);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void AttackCommandHandler_Throws_Exception_For_Self() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var player);

            var handler = new AttackCommandHandler(new PlayerRepository(builder.Context));
            var command = new AttackCommand("Raid", "test1@test.com", "1", "10");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            player.DidNotReceiveWithAnyArgs().ExecuteAttack(default, default, default);
        }

        [TestMethod]
        public void AttackCommandHandler_Throws_Exception_For_Alliance_Member() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1, out var attacker)
                .WithMember(2, out var defender);

            var handler = new AttackCommandHandler(new PlayerRepository(builder.Context));
            var command = new AttackCommand("Raid", "test1@test.com", "2", "10");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            attacker.DidNotReceiveWithAnyArgs().ExecuteAttack(default, default, default);
        }

        [TestMethod]
        public void AttackCommandHandler_Throws_Exception_For_Alphanumeric_Defender() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var attacker)
                .WithPlayer(2, out var defender);

            var handler = new AttackCommandHandler(new PlayerRepository(builder.Context));
            var command = new AttackCommand("Raid", "test1@test.com", "A", "10");

            Action action = () => handler.Execute(command);

            action.Should().Throw<FormatException>();
            attacker.DidNotReceiveWithAnyArgs().ExecuteAttack(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void AttackCommandHandler_Throws_Exception_For_Nonexistent_Defender() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var attacker)
                .WithPlayer(2, out var defender);

            var handler = new AttackCommandHandler(new PlayerRepository(builder.Context));
            var command = new AttackCommand("Raid", "test1@test.com", "5", "10");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            attacker.DidNotReceiveWithAnyArgs().ExecuteAttack(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void AttackCommandHandler_Throws_Exception_For_Nonexistent_Type() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var attacker)
                .WithPlayer(2, out var defender);

            var handler = new AttackCommandHandler(new PlayerRepository(builder.Context));
            var command = new AttackCommand("wrong", "test1@test.com", "5", "10");

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();
            attacker.DidNotReceiveWithAnyArgs().ExecuteAttack(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [DataTestMethod]
        [DataRow("Raid", AttackType.Raid, DisplayName = "Raid")]
        [DataRow("Assault", AttackType.Assault, DisplayName = "Assault")]
        public void AttackCommandHandler_Resolves_Type_Parameter_To_Correct_Type(string typeParameter, AttackType attackType) {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var attacker)
                .WithPlayer(2, out var defender);

            var handler = new AttackCommandHandler(new PlayerRepository(builder.Context));
            var command = new AttackCommand(typeParameter, "test1@test.com", "2", "10");

            handler.Execute(command);

            attacker.Received().ExecuteAttack(attackType, defender, 10);
        }

        [TestMethod]
        public void AttackCommandHandler_Fails_For_AlphaNumeric_Turns() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var attacker)
                .WithPlayer(2);

            var handler = new AttackCommandHandler(new PlayerRepository(builder.Context));
            var command = new AttackCommand("Raid", "test1@test.com", "2", "A");

            var result = handler.Execute(command);

            result.Should().HaveError("Turns", "Turns must be a valid number");
            attacker.DidNotReceiveWithAnyArgs().ExecuteAttack(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void AttackCommandHandler_Fails_For_Too_Few_Turns_Available() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var attacker, attackTurns: 9)
                .WithPlayer(2);

            var handler = new AttackCommandHandler(new PlayerRepository(builder.Context));
            var command = new AttackCommand("Raid", "test1@test.com", "2", "10");

            var result = handler.Execute(command);

            result.Should().HaveError("Turns", "You don't have enough attack turns");
            attacker.DidNotReceiveWithAnyArgs().ExecuteAttack(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void AttackCommandHandler_Fails_For_Less_Than_One_Turn() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var attacker)
                .WithPlayer(2);

            var handler = new AttackCommandHandler(new PlayerRepository(builder.Context));
            var command = new AttackCommand("Raid", "test1@test.com", "2", "0");

            var result = handler.Execute(command);

            result.Should().HaveError("Turns", "Turns must be a valid number");
            attacker.DidNotReceiveWithAnyArgs().ExecuteAttack(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void AttackCommandHandler_Fails_For_More_Than_Ten_Turns() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var attacker)
                .WithPlayer(2);

            var handler = new AttackCommandHandler(new PlayerRepository(builder.Context));
            var command = new AttackCommand("Raid", "test1@test.com", "2", "11");

            var result = handler.Execute(command);

            result.Should().HaveError("Turns", "Turns must be a valid number");
            attacker.DidNotReceiveWithAnyArgs().ExecuteAttack(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}