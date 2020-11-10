using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.CommandHandlers.Alliances;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Alliances {
    [TestClass]
    public sealed class ReadInviteCommandHandlerTests {
        [TestMethod]
        public void ReadInviteCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var player)
                .BuildAlliance(1)
                .WithInvite(3, out var invite, player);

            var handler = new ReadInviteCommandHandler(new PlayerRepository(builder.Context));
            var command = new ReadInviteCommand("test1@test.com", 3);

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            invite.Received().IsRead = true;
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }
    }
}