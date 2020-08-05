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
    public sealed class GetNotificationsQueryHandlerTests {
        [TestMethod]
        public void GetNotificationsQueryHandler_Returns_All_False_By_Default() {
            var builder = new FakeBuilder().BuildPlayer(1);

            builder.BuildAlliance(1)
                .WithInvite(1, builder.Player, isRead: true)
                .BuildMember(2)
                .WithMessageTo(1, builder.Player, new DateTime(2020, 1, 2), isRead: true)
                .BuildAttackOn(1, builder.Player, AttackType.Raid, AttackResult.Won, isRead: true);

            builder.Player.HasUpkeepRunOut.Returns(false);
            builder.Player.WillUpkeepRunOut().Returns(false);
            builder.Player.HasNewMarketSales.Returns(false);
            builder.Player.GetAvailableHousingCapacity().Returns(5);
            builder.Player.GetTheoreticalRecruitsPerDay().Returns(5);
            builder.Player.GetSoldierRecruitsPenalty().Returns(0);

            var handler = new GetNotificationsQueryHandler(builder.Context);
            var query = new GetNotificationsQuery("test1@test.com");

            var result = handler.Execute(query);

            result.HasNewInvites.Should().BeFalse();
            result.HasNewMessages.Should().BeFalse();
            result.HasNewAttacks.Should().BeFalse();
            result.HasNewMarketSales.Should().BeFalse();
            result.HasHousingShortage.Should().BeFalse();
            result.HasUpkeepShortage.Should().BeFalse();
            result.HasSoldierShortage.Should().BeFalse();
        }

        [TestMethod]
        public void GetNotificationsQueryHandler_Returns_True_For_New_Notifications() {
            var builder = new FakeBuilder().BuildPlayer(1);

            builder.BuildAlliance(1)
                .WithInvite(1, builder.Player)
                .BuildMember(2)
                .WithMessageTo(1, builder.Player, new DateTime(2020, 1, 2))
                .BuildAttackOn(1, builder.Player, AttackType.Raid, AttackResult.Won);

            builder.Player.HasUpkeepRunOut.Returns(true);
            builder.Player.WillUpkeepRunOut().Returns(false);
            builder.Player.HasNewMarketSales.Returns(true);
            builder.Player.GetAvailableHousingCapacity().Returns(4);
            builder.Player.GetTheoreticalRecruitsPerDay().Returns(5);
            builder.Player.GetSoldierRecruitsPenalty().Returns(1);

            var handler = new GetNotificationsQueryHandler(builder.Context);
            var query = new GetNotificationsQuery("test1@test.com");

            var result = handler.Execute(query);

            result.HasNewInvites.Should().BeTrue();
            result.HasNewMessages.Should().BeTrue();
            result.HasNewAttacks.Should().BeTrue();
            result.HasNewMarketSales.Should().BeTrue();
            result.HasHousingShortage.Should().BeTrue();
            result.HasUpkeepShortage.Should().BeTrue();
            result.HasSoldierShortage.Should().BeTrue();
        }

        [TestMethod]
        public void GetNotificationsQueryHandler_HasUpkeepShortage_Returns_True_For_Upkeep_Running_Out() {
            var builder = new FakeBuilder().BuildPlayer(1);

            builder.Player.HasUpkeepRunOut.Returns(false);
            builder.Player.WillUpkeepRunOut().Returns(true);

            var handler = new GetNotificationsQueryHandler(builder.Context);
            var query = new GetNotificationsQuery("test1@test.com");

            var result = handler.Execute(query);

            result.HasUpkeepShortage.Should().BeTrue();
        }
    }
}