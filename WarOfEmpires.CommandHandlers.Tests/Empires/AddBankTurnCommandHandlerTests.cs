using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class AddBankTurnCommandHandlerTests {
        [TestMethod]
        public void AddBankTurnCommandHandler_Calls_AddBankTurn_For_All_Active_Players() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var player)
                .WithPlayer(2, out var anotherPlayer)
                .WithPlayer(3, out var inactivePlayer, status: UserStatus.Inactive);

            var handler = new AddBankTurnCommandHandler(new PlayerRepository(builder.Context));
            var command = new AddBankTurnCommand();

            handler.Execute(command);

            player.Received().AddBankTurn();
            anotherPlayer.Received().AddBankTurn();
            inactivePlayer.DidNotReceiveWithAnyArgs().AddBankTurn();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }
    }
}