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
    public sealed class GetAllianceHomeQueryHandlerTests {
        [TestMethod]
        public void GetAllianceHomeQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder().BuildAlliance(1);
            var player = builder.BuildLeader(3, rank: 2, lastOnline: new DateTime(2020, 1, 10)).Player;

            builder.BuildMember(1, rank: 3);
            builder.BuildMember(2, status: UserStatus.Inactive);
            builder.WithRole(1, "Superstar", player);

            var handler = new GetAllianceHomeQueryHandler(builder.Context);
            var query = new GetAllianceHomeQuery("test1@test.com");

            var result = handler.Execute(query);

            result.Id.Should().Be(1);
            result.Code.Should().Be("FS");
            result.Name.Should().Be("Føroyskir Samgonga");
            result.LeaderId.Should().Be(3);
            result.Leader.Should().Be("Test display name 3");
            result.Members.Should().HaveCount(2);
            result.Members.First().Id.Should().Be(3);
            result.Members.First().LastOnline.Should().Be(new DateTime(2020, 1, 10));
            result.Members.First().Rank.Should().Be(2);
            result.Members.First().DisplayName.Should().Be("Test display name 3");
            result.Members.First().Role.Should().Be("Superstar");
        }

        [TestMethod]
        public void GetAllianceHomeQueryHandler_Returns_Only_Recent_ChatMessages() {
            var builder = new FakeBuilder().BuildAlliance(1);

            builder.BuildMember(1)
                .WithChatMessage(new DateTime(2020, 2, 2), "Hidden")
                .WithChatMessage(DateTime.UtcNow.Date, "Displayed");
            builder.BuildMember(2, status: UserStatus.Inactive)
                .WithChatMessage(DateTime.UtcNow.Date.AddDays(-1), "Visible");
            builder.BuildLeader(3);

            var handler = new GetAllianceHomeQueryHandler(builder.Context);
            var query = new GetAllianceHomeQuery("test1@test.com");

            var result = handler.Execute(query);

            result.ChatMessages.Should().HaveCount(2);
            result.ChatMessages.First().PlayerId.Should().Be(1);
            result.ChatMessages.First().Player.Should().Be("Test display name 1");
            result.ChatMessages.First().Message.Should().Be("Displayed");
            result.ChatMessages.Last().PlayerId.Should().Be(null);
            result.ChatMessages.Last().Player.Should().Be("Test display name 2");
            result.ChatMessages.Last().Message.Should().Be("Visible");
        }
    }
}