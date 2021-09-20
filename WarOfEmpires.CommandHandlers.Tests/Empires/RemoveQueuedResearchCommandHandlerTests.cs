using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class RemoveQueuedResearchCommandHandlerTests {
        [TestMethod]
        public void RemoveQueuedResearchCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithQueuedResearch(1, out var queuedResearch, ResearchType.CombatMedicine, 1);

            var handler = new RemoveQueuedResearchCommandHandler(new PlayerRepository(builder.Context));
            var command = new RemoveQueuedResearchCommand("test1@test.com", 1);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.Received().RemoveQueuedResearch(queuedResearch);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void RemoveQueuedResearchCommandHandler_Throws_Exception_For_Invalid_QueuedResearchId() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithQueuedResearch(1, ResearchType.CombatMedicine, 1);

            var handler = new RemoveQueuedResearchCommandHandler(new PlayerRepository(builder.Context));
            var command = new RemoveQueuedResearchCommand("test1@test.com", 51);

            Action commandAction = () => handler.Execute(command);

            commandAction.Should().Throw<InvalidOperationException>();
            builder.Player.DidNotReceiveWithAnyArgs().RemoveQueuedResearch(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void RemoveQueuedResearchCommandHandler_Throws_Exception_For_QueuedResearch_Of_Different_Player() {
            var builder = new FakeBuilder()
                .WithPlayer(2, email: "wrong@test.com")
                .BuildPlayer(1)
                .WithQueuedResearch(1, ResearchType.CombatMedicine, 1);

            var handler = new RemoveQueuedResearchCommandHandler(new PlayerRepository(builder.Context));
            var command = new RemoveQueuedResearchCommand("wrong@test.com", 1);

            Action commandAction = () => handler.Execute(command);

            commandAction.Should().Throw<InvalidOperationException>();
            builder.Player.DidNotReceiveWithAnyArgs().RemoveQueuedResearch(default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}
