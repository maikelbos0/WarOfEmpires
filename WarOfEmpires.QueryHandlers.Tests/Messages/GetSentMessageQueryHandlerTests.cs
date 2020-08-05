using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.QueryHandlers.Messages;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Messages {
    [TestClass]
    public sealed class GetSentMessageQueryHandlerTests {
        [TestMethod]
        public void GetSentMessageQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder().BuildPlayer(1);

            builder.WithMessageTo(1, builder.BuildPlayer(2, email: "recipient@test.com", displayName: "Recipient test").Player, new DateTime(2019, 1, 1));

            var handler = new GetSentMessageQueryHandler(builder.Context);
            var query = new GetSentMessageQuery("test1@test.com", "1");

            var result = handler.Execute(query);

            result.Subject.Should().Be("Message subject");
            result.Body.Should().Be("Message body");
            result.Recipient.Should().Be("Recipient test");
            result.Date.Should().Be(new DateTime(2019, 1, 1));
            result.IsRead.Should().BeFalse();
        }

        [TestMethod]
        public void GetSentMessageQueryHandler_Throws_Exception_For_Message_Sent_To_Different_Player() {
            var builder = new FakeBuilder().BuildPlayer(1);

            builder.WithMessageTo(1, builder.BuildPlayer(2, email: "recipient@test.com").Player, new DateTime(2019, 1, 1));

            var handler = new GetSentMessageQueryHandler(builder.Context);
            var query = new GetSentMessageQuery("recipient@test.com", "1");

            Action action = () => handler.Execute(query);

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void GetSentMessageQueryHandler_Throws_Exception_For_Alphanumeric_MessageId() {
            var builder = new FakeBuilder().BuildPlayer(1);

            builder.WithMessageTo(1, builder.BuildPlayer(2, email: "recipient@test.com").Player, new DateTime(2019, 1, 1));

            var handler = new GetSentMessageQueryHandler(builder.Context);
            var query = new GetSentMessageQuery("test1@test.com", "A");

            Action action = () => handler.Execute(query);

            action.Should().Throw<FormatException>();
        }
    }
}