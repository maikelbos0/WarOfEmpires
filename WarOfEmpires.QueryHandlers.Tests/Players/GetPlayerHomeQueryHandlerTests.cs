using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Players {
    [TestClass]
    public sealed class GetPlayerHomeQueryHandlerTests {
        [TestMethod]
        public void GetPlayerHomeQueryHandler_Returns_All_False_By_Default() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var player)
                .BuildAlliance(1)
                .WithInvite(1, player, isRead: true)
                .BuildMember(2)
                .WithMessageTo(1, player, new DateTime(2020, 1, 2), isRead: true)
                .BuildAttackOn(1, player, AttackType.Raid, AttackResult.Won, isRead: true);

            player.HasUpkeepRunOut.Returns(false);
            player.WillUpkeepRunOut().Returns(false);
            player.HasNewMarketSales.Returns(false);
            player.HasNewChatMessages.Returns(false);
            player.GetAvailableHousingCapacity().Returns(5);
            player.GetTheoreticalRecruitsPerDay().Returns(5);
            player.GetSoldierRecruitsPenalty().Returns(0);

            var handler = new GetPlayerHomeQueryHandler(builder.Context);
            var query = new GetPlayerHomeQuery("test1@test.com");

            var result = handler.Execute(query);

            result.HasNewInvites.Should().BeFalse();
            result.HasNewMessages.Should().BeFalse();
            result.HasNewMarketSales.Should().BeFalse();
            result.HasNewChatMessages.Should().BeFalse();
            result.HasHousingShortage.Should().BeFalse();
            result.WillUpkeepRunOut.Should().BeFalse();
            result.HasUpkeepRunOut.Should().BeFalse();
            result.HasSoldierShortage.Should().BeFalse();
        }

        [TestMethod]
        public void GetPlayerHomeQueryHandler_Returns_True_For_New_Notifications() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var player)
                .BuildAlliance(1)
                .WithInvite(1, player)
                .BuildMember(2)
                .WithMessageTo(1, player, new DateTime(2020, 1, 2))
                .BuildAttackOn(1, player, AttackType.Raid, AttackResult.Won);

            player.HasUpkeepRunOut.Returns(true);
            player.WillUpkeepRunOut().Returns(false);
            player.HasNewMarketSales.Returns(true);
            player.HasNewChatMessages.Returns(true);
            player.GetAvailableHousingCapacity().Returns(4);
            player.GetTheoreticalRecruitsPerDay().Returns(5);
            player.GetSoldierRecruitsPenalty().Returns(1);
            
            var handler = new GetPlayerHomeQueryHandler(builder.Context);
            var query = new GetPlayerHomeQuery("test1@test.com");

            var result = handler.Execute(query);

            result.HasNewInvites.Should().BeTrue();
            result.HasNewMessages.Should().BeTrue();
            result.HasNewMarketSales.Should().BeTrue();
            result.HasNewChatMessages.Should().BeTrue();
            result.HasHousingShortage.Should().BeTrue();
            result.HasUpkeepRunOut.Should().BeTrue();
            result.HasSoldierShortage.Should().BeTrue();
        }

        [TestMethod]
        public void GetPlayerHomeQueryHandler_Returns_Correct_Other_Information() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithPeasants(9);

            builder
                .BuildPlayer(2)
                .BuildAttackOn(2, builder.Player, AttackType.Assault, AttackResult.Won)
                .WithRound(true, new Casualties(TroopType.Archers, 1, 2))
                .WithRound(true, new Casualties(TroopType.Cavalry, 3, 4))
                .WithRound(true, new Casualties(TroopType.Footmen, 5, 6));

            builder
                .BuildAttackOn(1, builder.Player, AttackType.Raid, AttackResult.Won)
                .WithRound(true, new Casualties(TroopType.Archers, 1, 2))
                .WithRound(true, new Casualties(TroopType.Cavalry, 3, 4))
                .WithRound(true, new Casualties(TroopType.Footmen, 5, 6));

            var handler = new GetPlayerHomeQueryHandler(builder.Context);
            var query = new GetPlayerHomeQuery("test1@test.com");

            var result = handler.Execute(query);

            result.DisplayName.Should().Be("Test display name 1");
            result.NewAttackCount.Should().Be(2);
            result.TotalSoldierCasualties.Should().Be(18);
            result.TotalMercenaryCasualties.Should().Be(24);
            result.CurrentPeasants.Should().Be(9);
        }

        [TestMethod]
        public void GetPlayerHomeQueryHandler_WillUpkeepRunOut_Returns_True_For_Upkeep_Running_Out() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            builder.Player.HasUpkeepRunOut.Returns(false);
            builder.Player.WillUpkeepRunOut().Returns(true);
            
            var handler = new GetPlayerHomeQueryHandler(builder.Context);
            var query = new GetPlayerHomeQuery("test1@test.com");

            var result = handler.Execute(query);

            result.WillUpkeepRunOut.Should().BeTrue();
        }
    }
}
