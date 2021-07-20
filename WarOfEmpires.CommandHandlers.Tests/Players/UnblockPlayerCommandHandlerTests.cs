using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.CommandHandlers.Players;
using WarOfEmpires.Commands.Players;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Players {
    [TestClass]
    public sealed class UnblockPlayerCommandHandlerTests {
        [TestMethod]
        public void UnblockPlayerCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .WithPlayer(2, out var blockedPlayer)
                .BuildPlayer(1);

            var handler = new UnblockPlayerCommandHandler(new PlayerRepository(builder.Context));
            var command = new UnblockPlayerCommand("test1@test.com", 2);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            builder.Player.Received().Unblock(blockedPlayer);
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }
    }
}
