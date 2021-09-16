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
    public sealed class QueueResearchCommandHandlerTests {
        [TestMethod]
        public void QueueResearchCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new QueueResearchCommandHandler(new PlayerRepository(builder.Context));
            var command = new QueueResearchCommand("test1@test.com", "Commerce");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.Received().QueueResearch(ResearchType.Commerce);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void QueueResearchCommandHandler_Throws_Exception_For_Invalid_ResearchType() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new QueueResearchCommandHandler(new PlayerRepository(builder.Context));
            var command = new QueueResearchCommand("test1@test.com", "Wrong");

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();
        }
    }
}
