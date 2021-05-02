using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class UpdateRankCommandHandlerTests {
        [TestMethod]
        public void UpdateRankCommandHandler_Calls_RankService_Update_For_Active_Players() {
            var rankService = Substitute.For<IRankService>();
            var builder = new FakeBuilder()
                .WithPlayer(1, out var player)
                .WithPlayer(2, out var anotherPlayer)
                .WithPlayer(3, out var inactivePlayer, status: UserStatus.Inactive)
                .WithPlayer(4, out var newPlayer, status: UserStatus.New);

            var handler = new UpdateRankCommandHandler(new PlayerRepository(builder.Context), rankService);
            var command = new UpdateRankCommand();
            
            handler.Execute(command);
            rankService.Received().Update(Arg.Do<IEnumerable<Player>>(players => {
                players.Should().HaveCount(2);
                players.Should().Contain(player);
                players.Should().Contain(anotherPlayer);
            }));
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }
    }
}