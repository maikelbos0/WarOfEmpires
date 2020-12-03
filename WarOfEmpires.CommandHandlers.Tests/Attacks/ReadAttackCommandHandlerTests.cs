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
    public sealed class ReadAttackCommandHandlerTests {
        [TestMethod]
        public void ReadAttackCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var defender, email: "defender@test.com")
                .BuildPlayer(2)
                .WithAttackOn(1, out var attack, defender, AttackType.Raid, AttackResult.Won);

            var handler = new ReadAttackCommandHandler(new PlayerRepository(builder.Context));
            var command = new ReadAttackCommand("defender@test.com", 1);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            attack.Received().IsRead = true;
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void ReadAttackCommandHandler_Throws_Exception_For_Attack_Received_By_Different_Player() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var defender)
                .BuildPlayer(2, email: "attacker@test.com")
                .WithAttackOn(1, out var attack, defender, AttackType.Raid, AttackResult.Won);

            var handler = new ReadAttackCommandHandler(new PlayerRepository(builder.Context));
            var command = new ReadAttackCommand("attacker@test.com", 1);

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            attack.DidNotReceiveWithAnyArgs().IsRead = default;
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}