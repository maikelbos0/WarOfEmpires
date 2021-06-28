using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.CommandHandlers.Game;
using WarOfEmpires.Commands.Game;
using WarOfEmpires.Domain.Game;
using WarOfEmpires.Repositories.Game;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Game {
    [TestClass]
    public sealed class SetGamePhaseCommandHandlerTests {
        [TestMethod]
        public void SetGamePhaseCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .WithGameStatus(1, out var gameStatus, phase: GamePhase.Active);

            var handler = new SetGamePhaseCommandHandler(new GameStatusRepository(builder.Context));
            var command = new SetGamePhaseCommand("Truce");

            handler.Execute(command);

            gameStatus.Received().Phase = GamePhase.Truce;
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void SetGamePhaseCommandHandler_Throws_Exception_For_Nonexistent_Type() {
            var builder = new FakeBuilder()
                .WithGameStatus(1, out var gameStatus, phase: GamePhase.Active);

            var handler = new SetGamePhaseCommandHandler(new GameStatusRepository(builder.Context));
            var command = new SetGamePhaseCommand("Wrong");

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();
            gameStatus.DidNotReceiveWithAnyArgs().Phase = default;
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}
