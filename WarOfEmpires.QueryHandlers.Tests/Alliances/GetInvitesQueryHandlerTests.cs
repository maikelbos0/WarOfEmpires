using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Alliances {
    [TestClass]
    public sealed class GetInvitesQueryHandlerTests {
        [TestMethod]
        public void GetInvitesQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .WithPlayer(4, out var activePlayer)
                .WithPlayer(5, out var inactivePlayer, status: UserStatus.Inactive)
                .BuildAlliance(1)
                .WithInvite(1, activePlayer, subject: "Message", isRead: false, date: new DateTime(2020, 2, 15))
                .WithInvite(2, inactivePlayer, subject: "Message", isRead: false, date: new DateTime(2020, 1, 2))
                .BuildLeader(1);

            var handler = new GetInvitesQueryHandler(builder.Context);
            var query = new GetInvitesQuery("test1@test.com");

            var result = handler.Execute(query).ToList();

            result.Should().HaveCount(1);
            result[0].Id.Should().Be(1);
            result[0].PlayerId.Should().Be(4);
            result[0].PlayerName.Should().Be("Test display name 4");
            result[0].IsRead.Should().BeFalse();
            result[0].Date.Should().Be(new DateTime(2020, 2, 15));
            result[0].Subject.Should().Be("Message");
        }
    }
}