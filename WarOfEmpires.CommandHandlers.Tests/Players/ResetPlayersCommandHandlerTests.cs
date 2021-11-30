using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.CommandHandlers.Players;
using WarOfEmpires.Commands.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Players {
    [TestClass]
    public sealed class ResetPlayersCommandHandlerTests {
        [TestMethod]
        public void ResetPlayersCommandHandler_Resets_All_Active_Players() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var activePlayer1, status: UserStatus.Active)
                .WithPlayer(2, out var activePlayer2, status: UserStatus.Active)
                .WithPlayer(3, out var newPlayer, status: UserStatus.New)
                .WithPlayer(4, out var inactivePlayer, status: UserStatus.Inactive);

            var handler = new ResetPlayersCommandHandler(new PlayerRepository(builder.Context), new AllianceRepository(builder.Context));
            var command = new ResetPlayersCommand();

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            activePlayer1.Received().Reset();
            activePlayer2.Received().Reset();
            newPlayer.DidNotReceiveWithAnyArgs().Reset();
            inactivePlayer.DidNotReceiveWithAnyArgs().Reset();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void ResetPlayersCommandHandler_Resets_All_Alliances() {
            var builder = new FakeBuilder()
                .WithAlliance(1, out var alliance1)
                .WithAlliance(1, out var alliance2);;

            var handler = new ResetPlayersCommandHandler(new PlayerRepository(builder.Context), new AllianceRepository(builder.Context));
            var command = new ResetPlayersCommand();

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            alliance1.Received().Reset();
            alliance2.Received().Reset();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }
    }
}
