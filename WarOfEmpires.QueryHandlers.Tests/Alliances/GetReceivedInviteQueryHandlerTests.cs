using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Alliances {
    [TestClass]
    public sealed class GetReceivedInviteQueryHandlerTests {
        [TestMethod]
        public void GetReceivedInviteQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var player)
                .BuildAlliance(3)
                .WithInvite(1, player)
                .BuildAlliance(4, code: "ANOT", name: "Another")
                .WithInvite(2, player, subject: "Invite from Another", body: "Hey, here's your invite into Another", isRead: true, date: new DateTime(2020, 1, 29));

            var handler = new GetReceivedInviteQueryHandler(builder.Context);
            var query = new GetReceivedInviteQuery("test1@test.com", 2);

            var result = handler.Execute(query);

            result.Id.Should().Be(2);
            result.AllianceId.Should().Be(4);
            result.AllianceCode.Should().Be("ANOT");
            result.AllianceName.Should().Be("Another");
            result.IsRead.Should().BeTrue();
            result.Date.Should().Be(new DateTime(2020, 1, 29));
            result.Subject.Should().Be("Invite from Another");
            result.Body.Should().Be("Hey, here's your invite into Another");
        }

        [TestMethod]
        public void GetReceivedInviteQueryHandler_Throws_Exception_For_Message_Received_By_Different_Player() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var player)
                .WithPlayer(2, email: "wrong@test.com")
                .BuildAlliance(3)
                .WithInvite(2, player);

            var handler = new GetReceivedInviteQueryHandler(builder.Context);
            var query = new GetReceivedInviteQuery("wrong@test.com", 2);

            Action action = () => handler.Execute(query);

            action.Should().Throw<InvalidOperationException>();
        }
    }
}