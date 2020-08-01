using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Alliances {
    [TestClass]
    public sealed class GetReceivedInvitesQueryHandlerTest {
        [TestMethod]
        public void GetReceivedInvitesQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder();
            var player = builder.BuildPlayer(1).Player;

            builder.BuildAlliance(4, code: "ANOT", name: "Another")
                .WithInvite(2, player, subject: "Invite from Another", isRead: true, date: new DateTime(2020, 1, 29));
            builder.BuildAlliance(3, code: "ALLI", name: "Allies")
                .WithInvite(1, player, subject: "Invite from Allies", isRead: true, date: new DateTime(2020, 1, 30));

            var handler = new GetReceivedInvitesQueryHandler(builder.Context);
            var query = new GetReceivedInvitesQuery("test1@test.com");

            var result = handler.Execute(query).ToList();

            result.Should().HaveCount(2);
            result[0].Id.Should().Be(2);
            result[0].AllianceId.Should().Be(4);
            result[0].AllianceCode.Should().Be("ANOT");
            result[0].AllianceName.Should().Be("Another");
            result[0].Date.Should().Be(new DateTime(2020, 1, 29));
            result[0].IsRead.Should().BeTrue();
            result[0].Subject.Should().Be("Invite from Another");
            result[1].Id.Should().Be(1);
            result[1].AllianceId.Should().Be(3);
            result[1].AllianceCode.Should().Be("ALLI");
            result[1].AllianceName.Should().Be("Allies");
            result[1].Date.Should().Be(new DateTime(2020, 1, 30));
            result[1].IsRead.Should().BeTrue();
            result[1].Subject.Should().Be("Invite from Allies");
        }
    }
}