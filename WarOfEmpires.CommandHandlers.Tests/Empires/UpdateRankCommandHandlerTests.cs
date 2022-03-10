using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Game;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Game;
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
                            .WithPlayer(4, out var newPlayer, status: UserStatus.New)
                            .WithGameStatus(1);

            var handler = new UpdateRankCommandHandler(new PlayerRepository(builder.Context), new GameStatusRepository(builder.Context), rankService);
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
            var builder = new FakeBuilder()
                            .WithPlayer(1, out var player, displayName: "The OG", title: TitleType.GrandOverlord, grandOverlordTime: TimeSpan.FromMinutes(1234))
                            .WithGameStatus(1, out var gameStatus);

            var handler = new UpdateRankCommandHandler(new PlayerRepository(builder.Context), new GameStatusRepository(builder.Context), rankService);
            var command = new UpdateRankCommand();

            handler.Execute(command);

            gameStatus.CurrentGrandOverlord.Should().Be(player);
        }

        [DataTestMethod]
        [DataRow(71, GamePhase.Active, DisplayName = "71 hours (active)")]
        [DataRow(72, GamePhase.Finished, DisplayName = "72 hours (finished)")]
        [DataRow(73, GamePhase.Finished, DisplayName = "73 hours (finished)")]
        public void UpdateRankCommandHandler_Ends_Game_If_Needed(int grandOverlordHours, GamePhase expectedPhase) {
            var rankService = Substitute.For<IRankService>();
            var builder = new FakeBuilder()
                            .WithPlayer(1, displayName: "The OG", title: TitleType.GrandOverlord, grandOverlordTime: TimeSpan.FromHours(grandOverlordHours))
                            .WithGameStatus(1, out var gameStatus, phase: GamePhase.Active);

            var handler = new UpdateRankCommandHandler(new PlayerRepository(builder.Context), new GameStatusRepository(builder.Context), rankService);
            var command = new UpdateRankCommand();

            handler.Execute(command);

            gameStatus.Phase.Should().Be(expectedPhase);
        }

        [TestMethod]
        public void UpdateRankCommandHandler_Leaves_GrandOverlord_If_Game_Finished() {
            var rankService = Substitute.For<IRankService>();
            var builder = new FakeBuilder()
                            .WithPlayer(1, out var originalGrandOverlord, displayName: "The OG", title: TitleType.Overlord, grandOverlordTime: TimeSpan.FromMinutes(1234))
                            .WithPlayer(2, displayName: "The New Hero", title: TitleType.GrandOverlord, grandOverlordTime: TimeSpan.FromMinutes(1234))
                            .WithGameStatus(1, out var gameStatus, grandOverlord: originalGrandOverlord, phase: GamePhase.Finished);

            var handler = new UpdateRankCommandHandler(new PlayerRepository(builder.Context), new GameStatusRepository(builder.Context), rankService);
            var command = new UpdateRankCommand();

            handler.Execute(command);

            gameStatus.CurrentGrandOverlord.Should().Be(originalGrandOverlord);
        }

        [TestMethod]
        public void UpdateRankCommandHandler_Clears_GrandOverlord_If_Not_Available() {
            var rankService = Substitute.For<IRankService>();
            var builder = new FakeBuilder()
                            .WithPlayer(1, out var player)
                            .WithGameStatus(1, out var gameStatus, grandOverlord: player);

            var handler = new UpdateRankCommandHandler(new PlayerRepository(builder.Context), new GameStatusRepository(builder.Context), rankService);
            var command = new UpdateRankCommand();

            handler.Execute(command);

            gameStatus.CurrentGrandOverlord.Should().BeNull();
        }
    }
}