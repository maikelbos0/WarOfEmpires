using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Alliances {
    [TestClass]
    public sealed class GetInviteQueryHandlerTests {
        [TestMethod]
        public void GetInviteQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .WithPlayer(4, out var player)
                .BuildAlliance(1)
                .WithInvite(1, player, isRead: false, date: new DateTime(2020, 2, 15))
                .WithLeader(1);

            var handler = new GetInviteQueryHandler(builder.Context);
            var query = new GetInviteQuery("test1@test.com", 1);

            var result = handler.Execute(query);

            result.Id.Should().Be(1);
            result.PlayerId.Should().Be(4);
            result.PlayerName.Should().Be("Test display name 4");
            result.IsRead.Should().BeFalse();
            result.Date.Should().Be(new DateTime(2020, 2, 15));
            result.Subject.Should().Be("Message subject");
            result.Body.Should().Be("Message body");
        }

        [TestMethod]
        public void GetInviteQueryHandler_Throws_Exception_For_Message_Sent_By_Different_Alliance() {
            var builder = new FakeBuilder()
                .WithPlayer(2, email: "wrong@test.com")
                .WithPlayer(4, out var player)
                .BuildAlliance(1)
                .WithInvite(1, player, isRead: false, date: new DateTime(2020, 2, 15))
                .WithLeader(1);

            var handler = new GetInviteQueryHandler(builder.Context);
            var query = new GetInviteQuery("wrong@test.com", 1);

            Action action = () => handler.Execute(query);

            action.Should().Throw<NullReferenceException>();
        }
    }
}