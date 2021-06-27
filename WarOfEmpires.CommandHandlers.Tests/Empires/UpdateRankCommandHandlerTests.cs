using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Game;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class UpdateRankCommandHandlerTests {
        [TestMethod]
        public void UpdateRankCommandHandler_Calls_RankService_Update_For_Active_Players() {
            var rankService = Substitute.For<IRankService>();
            var gameStatus = new GameStatus();
            var builder = new FakeBuilder()
                            .WithPlayer(1, out var player)
                            .WithPlayer(2, out var anotherPlayer)
                            .WithPlayer(3, out var inactivePlayer, status: UserStatus.Inactive)
                            .WithPlayer(4, out var newPlayer, status: UserStatus.New);

            var handler = new UpdateRankCommandHandler(new PlayerRepository(builder.Context), rankService, gameStatus);
            var command = new UpdateRankCommand();

            handler.Execute(command);
            rankService.Received().Update(Arg.Do<IEnumerable<Player>>(players => {
                players.Should().HaveCount(2);
                players.Should().Contain(player);
                players.Should().Contain(anotherPlayer);
            }));
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }


        [TestMethod]
        public void UpdateRankCommandHandler_Sets_GrandOverlord_If_Available() {
            var rankService = Substitute.For<IRankService>();
            var gameStatus = new GameStatus();
            var builder = new FakeBuilder()
                            .WithPlayer(1, displayName: "The OG", title: TitleType.GrandOverlord, grandOverlordTime: TimeSpan.FromMinutes(1234));

            var handler = new UpdateRankCommandHandler(new PlayerRepository(builder.Context), rankService, gameStatus);
            var command = new UpdateRankCommand();

            handler.Execute(command);

            gameStatus.CurrentGrandOverlordId.Should().Be(1);
            gameStatus.CurrentGrandOverlord.Should().Be("The OG");
            gameStatus.CurrentGrandOverlordTime.Should().Be(TimeSpan.FromMinutes(1234));
        }


        [TestMethod]
        public void UpdateRankCommandHandler_Clears_GrandOverlord_If_Not_Available() {
            var rankService = Substitute.For<IRankService>();
            var gameStatus = new GameStatus() { CurrentGrandOverlordId = 1, CurrentGrandOverlord = "The OG", CurrentGrandOverlordTime = TimeSpan.FromMinutes(1234) };
            var builder = new FakeBuilder()
                            .WithPlayer(1);

            var handler = new UpdateRankCommandHandler(new PlayerRepository(builder.Context), rankService, gameStatus);
            var command = new UpdateRankCommand();

            handler.Execute(command);

            gameStatus.CurrentGrandOverlordId.Should().BeNull();
            gameStatus.CurrentGrandOverlord.Should().BeNull();
            gameStatus.CurrentGrandOverlordTime.Should().BeNull();
        }
    }
}