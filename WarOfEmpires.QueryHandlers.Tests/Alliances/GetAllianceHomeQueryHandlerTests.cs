using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Alliances;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Tests.Alliances {
    [TestClass]
    public sealed class GetAllianceHomeQueryHandlerTests {
        [TestMethod]
        public void GetAllianceHomeQueryHandler_Returns_Correct_Information() {
            var context = new FakeWarContext();
            var builder = new FakeBuilder(context).CreateAlliance(1);

            builder.CreatePlayer(1, rank: 3);
            builder.CreatePlayer(2, status: UserStatus.Inactive);
            builder.CreateLeader(3, rank: 2, lastOnline: new DateTime(2020, 1, 10)).AddPopulation();

            var handler = new GetAllianceHomeQueryHandler(context, new EnumFormatter());
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
            result.Members.First().Title.Should().Be("Sub chieftain");
            result.Members.First().Population.Should().Be(49);
        }

        [TestMethod]
        public void GetAllianceHomeQueryHandler_Returns_Only_Recent_ChatMessages() {
            var context = new FakeWarContext();
            var builder = new FakeBuilder(context).CreateAlliance(1);

            builder.CreatePlayer(1)
                .AddChatMessage(new DateTime(2020, 2, 2), "Hidden")
                .AddChatMessage(DateTime.UtcNow.Date, "Displayed");
            builder.CreatePlayer(2, status: UserStatus.Inactive)
                .AddChatMessage(DateTime.UtcNow.Date.AddDays(-1), "Visible");
            builder.CreateLeader(3);

            var handler = new GetAllianceHomeQueryHandler(context, new EnumFormatter());
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