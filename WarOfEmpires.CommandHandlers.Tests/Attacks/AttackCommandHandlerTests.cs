using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.CommandHandlers.Attacks;
using WarOfEmpires.Commands.Attacks;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Game;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Repositories.Game;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Attacks {
    [TestClass]
    public sealed class AttackCommandHandlerTests {
        [TestMethod]
        public void AttackCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .WithGameStatus(1)
                .WithPlayer(1, out var attacker)
                .WithPlayer(2, out var defender);

            var handler = new AttackCommandHandler(new PlayerRepository(builder.Context), new GameStatusRepository(builder.Context));
            var command = new AttackCommand("Raid", "test1@test.com", 2, 10);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            attacker.Received().ExecuteAttack(AttackType.Raid, defender, 10);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void AttackCommandHandler_Throws_Exception_For_Truce() {
            var builder = new FakeBuilder()
                .WithGameStatus(1, phase: GamePhase.Truce)
                .WithPlayer(1, out var player);

            var handler = new AttackCommandHandler(new PlayerRepository(builder.Context), new GameStatusRepository(builder.Context));
            var command = new AttackCommand("Raid", "test1@test.com", 1, 10);

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            player.DidNotReceiveWithAnyArgs().ExecuteAttack(default, default, default);
        }

        [TestMethod]
        public void AttackCommandHandler_Throws_Exception_For_Self() {
            var builder = new FakeBuilder()
                .WithGameStatus(1)
                .WithPlayer(1, out var player);

            var handler = new AttackCommandHandler(new PlayerRepository(builder.Context), new GameStatusRepository(builder.Context));
            var command = new AttackCommand("Raid", "test1@test.com", 1, 10);

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            player.DidNotReceiveWithAnyArgs().ExecuteAttack(default, default, default);
        }

        [TestMethod]
        public void AttackCommandHandler_Throws_Exception_For_Alliance_Member() {
            var builder = new FakeBuilder()
                .WithGameStatus(1)
                .BuildAlliance(1)
                .WithMember(1, out var attacker)
                .WithMember(2, out var defender);

            var handler = new AttackCommandHandler(new PlayerRepository(builder.Context), new GameStatusRepository(builder.Context));
            var command = new AttackCommand("Raid", "test1@test.com", 2, 10);

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            attacker.DidNotReceiveWithAnyArgs().ExecuteAttack(default, default, default);
        }

        [TestMethod]
        public void AttackCommandHandler_Throws_Exception_For_Pact() {
            var builder = new FakeBuilder()
                .WithGameStatus(1)
                .BuildAlliance(1)
                .WithMember(1, out var attacker);

            builder.BuildAlliance(2)
                .WithMember(2, out var defender)
                .WithNonAggressionPact(1, builder.Alliance);

            var handler = new AttackCommandHandler(new PlayerRepository(builder.Context), new GameStatusRepository(builder.Context));
            var command = new AttackCommand("Raid", "test1@test.com", 2, 10);

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            attacker.DidNotReceiveWithAnyArgs().ExecuteAttack(default, default, default);
        }

        [TestMethod]
        public void AttackCommandHandler_Throws_Exception_For_Nonexistent_Defender() {
            var builder = new FakeBuilder()
                .WithGameStatus(1)
                .WithPlayer(1, out var attacker)
                .WithPlayer(2, out var defender);

            var handler = new AttackCommandHandler(new PlayerRepository(builder.Context), new GameStatusRepository(builder.Context));
            var command = new AttackCommand("Raid", "test1@test.com", 5, 10);

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            attacker.DidNotReceiveWithAnyArgs().ExecuteAttack(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void AttackCommandHandler_Throws_Exception_For_Nonexistent_Type() {
            var builder = new FakeBuilder()
                .WithGameStatus(1)
                .WithPlayer(1, out var attacker)
                .WithPlayer(2, out var defender);

            var handler = new AttackCommandHandler(new PlayerRepository(builder.Context), new GameStatusRepository(builder.Context));
            var command = new AttackCommand("wrong", "test1@test.com", 5, 10);

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();
            attacker.DidNotReceiveWithAnyArgs().ExecuteAttack(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [DataTestMethod]
        [DataRow("Raid", AttackType.Raid, DisplayName = "Raid")]
        [DataRow("Assault", AttackType.Assault, DisplayName = "Assault")]
        [DataRow("GrandOverlordAttack", AttackType.GrandOverlordAttack, DisplayName = "GrandOverlordAttack")]
        public void AttackCommandHandler_Resolves_Type_Parameter_To_Correct_Type(string typeParameter, AttackType expectedAttackType) {
            var builder = new FakeBuilder()
                .WithGameStatus(1)
                .WithPlayer(1, out var attacker)
                .WithPlayer(2, out var defender, title: TitleType.GrandOverlord);

            var handler = new AttackCommandHandler(new PlayerRepository(builder.Context), new GameStatusRepository(builder.Context));
            var command = new AttackCommand(typeParameter, "test1@test.com", 2, 10);

            handler.Execute(command);

            attacker.Received().ExecuteAttack(expectedAttackType, defender, 10);
        }

        [TestMethod]
        public void AttackCommandHandler_Fails_For_Too_Few_Turns_Available() {
            var builder = new FakeBuilder()
                .WithGameStatus(1)
                .WithPlayer(1, out var attacker, attackTurns: 9)
                .WithPlayer(2);

            var handler = new AttackCommandHandler(new PlayerRepository(builder.Context), new GameStatusRepository(builder.Context));
            var command = new AttackCommand("Raid", "test1@test.com", 2, 10);

            var result = handler.Execute(command);

            result.Should().HaveError(c => c.Turns, "You don't have enough attack turns");
            attacker.DidNotReceiveWithAnyArgs().ExecuteAttack(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void AttackCommandHandler_Fails_For_GrandOverlordAttack_On_Not_GrandOverlord() {
            var builder = new FakeBuilder()
                .WithGameStatus(1)
                .WithPlayer(1, out var attacker)
                .WithPlayer(2);

            var handler = new AttackCommandHandler(new PlayerRepository(builder.Context), new GameStatusRepository(builder.Context));
            var command = new AttackCommand("GrandOverlordAttack", "test1@test.com", 2, 10);

            var result = handler.Execute(command);

            result.Should().HaveError("Your opponent is not the Grand Overlord");
            attacker.DidNotReceiveWithAnyArgs().ExecuteAttack(default, default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}