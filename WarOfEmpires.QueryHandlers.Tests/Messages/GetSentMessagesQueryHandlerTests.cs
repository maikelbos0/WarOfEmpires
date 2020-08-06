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
            var builder = new FakeBuilder()
                .WithPlayer(2, out var recipient)
                .WithPlayer(3, out var inactiveRecipient, status: UserStatus.Inactive)
                .BuildPlayer(1)
                .WithMessageTo(1, recipient, new DateTime(2019, 1, 1))
                .WithMessageTo(2, inactiveRecipient, new DateTime(2020, 2, 1));

            var handler = new GetSentMessagesQueryHandler(builder.Context);
            var query = new GetSentMessagesQuery("test1@test.com");

            var result = handler.Execute(query);

            result.Count().Should().Be(2);
        }

        [TestMethod]
        public void GetSentMessagesQueryHandler_Returns_Correct_Message_Data() {
            var builder = new FakeBuilder()
                .WithPlayer(2, out var recipient, displayName: "Recipient test")
                .BuildPlayer(1)
                .WithMessageTo(1, recipient, new DateTime(2019, 1, 1));

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