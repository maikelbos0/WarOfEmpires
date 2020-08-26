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
    public sealed class RecruitCommandHandlerTests {
        [TestMethod]
        public void RecruitCommandHandler_Calls_Recruit_For_All_Active_Players() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var player)
                .WithPlayer(2, out var anotherPlayer)
                .WithPlayer(3, out var inactivePlayer, status: UserStatus.Inactive);

            var handler = new RecruitCommandHandler(new PlayerRepository(builder.Context));
            var command = new RecruitCommand();

            handler.Execute(command);

            player.Received().Recruit();
            anotherPlayer.Received().Recruit();
            inactivePlayer.DidNotReceiveWithAnyArgs().Recruit();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }
    }
}