using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.CommandHandlers.Alliances;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Alliances {
    [TestClass]
    public sealed class ReadNewChatMessagesCommandHandlerTests {
        [TestMethod]
        public void ReadNewChatMessagesCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new ReadNewChatMessagesCommandHandler(new PlayerRepository(builder.Context));
            var command = new ReadNewChatMessagesCommand("test1@test.com");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.Received().HasNewChatMessages = false;
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }
    }
}