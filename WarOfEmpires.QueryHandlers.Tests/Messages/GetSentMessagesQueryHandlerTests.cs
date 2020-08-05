using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.QueryHandlers.Messages;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Messages {
    [TestClass]
    public sealed class GetSentMessagesQueryHandlerTests {
        [TestMethod]
        public void GetSentMessagesQueryHandler_Returns_All_Sent_Messages() {
            var builder = new FakeBuilder().BuildPlayer(1);

            builder.WithMessageTo(1, builder.BuildPlayer(2).Player, new DateTime(2019, 1, 1))
                .WithMessageTo(2, builder.BuildPlayer(3).Player, new DateTime(2020, 2, 1))
                .WithMessageTo(3, builder.BuildPlayer(4, status: UserStatus.Inactive).Player, new DateTime(2020, 3, 1));

            var handler = new GetSentMessagesQueryHandler(builder.Context);
            var query = new GetSentMessagesQuery("test1@test.com");

            var result = handler.Execute(query);

            result.Count().Should().Be(3);
        }

        [TestMethod]
        public void GetSentMessagesQueryHandler_Returns_Correct_Message_Data() {
            var builder = new FakeBuilder().BuildPlayer(1);

            builder.WithMessageTo(1, builder.BuildPlayer(2, displayName: "Recipient test").Player, new DateTime(2019, 1, 1));

            var handler = new GetSentMessagesQueryHandler(builder.Context);
            var query = new GetSentMessagesQuery("test1@test.com");

            var result = handler.Execute(query);
            var message = result.FirstOrDefault();

            message.Should().NotBeNull();
            message.Recipient.Should().Be("Recipient test");
            message.Date.Should().Be(new DateTime(2019,1,1));
            message.IsRead.Should().BeFalse();
            message.Subject.Should().Be("Message subject");
        }
    }
}