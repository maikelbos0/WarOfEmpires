using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WarOfEmpires.QueryHandlers.Tests.Players {
    [TestClass]
    public sealed class GetPlayerHomeQueryHandlerTests {
        [TestMethod]
        public void GetPlayerHomeQueryHandler_Returns_All_False_By_Default() {
            /*
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

            var handler = new GetNotificationsQueryHandler(builder.Context);
            var query = new GetNotificationsQuery("test1@test.com");

            var result = handler.Execute(query);

            result.HasNewInvites.Should().BeFalse();
            result.HasNewMessages.Should().BeFalse();
            result.HasNewAttacks.Should().BeFalse();
            result.HasNewMarketSales.Should().BeFalse();
            result.HasNewChatMessages.Should().BeFalse();
            result.HasHousingShortage.Should().BeFalse();
            result.HasUpkeepShortage.Should().BeFalse();
            result.HasSoldierShortage.Should().BeFalse();
            */
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void GetPlayerHomeQueryHandler_Returns_True_For_New_Notifications() {
            /*
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

            var handler = new GetNotificationsQueryHandler(builder.Context);
            var query = new GetNotificationsQuery("test1@test.com");

            var result = handler.Execute(query);

            result.HasNewInvites.Should().BeTrue();
            result.HasNewMessages.Should().BeTrue();
            result.HasNewAttacks.Should().BeTrue();
            result.HasNewMarketSales.Should().BeTrue();
            result.HasNewChatMessages.Should().BeTrue();
            result.HasHousingShortage.Should().BeTrue();
            result.HasUpkeepShortage.Should().BeTrue();
            result.HasSoldierShortage.Should().BeTrue();
            */
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void GetPlayerHomeQueryHandler_Returns_Correct_Attack_Information() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void GetPlayerHomeQueryHandler_HasUpkeepShortage_Returns_True_For_Upkeep_Running_Out() {
            /*
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            builder.Player.HasUpkeepRunOut.Returns(false);
            builder.Player.WillUpkeepRunOut().Returns(true);

            var handler = new GetNotificationsQueryHandler(builder.Context);
            var query = new GetNotificationsQuery("test1@test.com");

            var result = handler.Execute(query);

            result.HasUpkeepShortage.Should().BeTrue();
            */
            throw new System.NotImplementedException();
        }
    }
}
