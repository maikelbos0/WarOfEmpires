using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.QueryHandlers.Messages;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Messages {
    [TestClass]
    public sealed class GetReplyToMessageQueryHandlerTests {
        [TestMethod]
        public void GetReplyToMessageQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .WithPlayer(2, out var player)
                .BuildPlayer(1, displayName: "Sender test")
                .WithMessageTo(1, player, new DateTime(2019, 1, 5, 14, 15, 33));

            var handler = new GetReplyToMessageQueryHandler(builder.Context);
            var query = new GetReplyToMessageQuery("test2@test.com", 1);

            var result = handler.Execute(query);

            result.RecipientId.Should().Be(1);
            result.Recipient.Should().Be("Sender test");
            result.Subject.Should().Be("Re: Message subject");
            result.Body.Should().Be($"{Environment.NewLine}Sender test wrote on 2019-01-05 14:15:{Environment.NewLine}Message body");
        }

        [TestMethod]
        public void GetReplyToMessageQueryHandler_Throws_Exception_For_Nonexistent_Id() {
            var builder = new FakeBuilder()
                .WithPlayer(2, out var player)
                .BuildPlayer(1, email: "sender@test.com")
                .WithMessageTo(1, player, new DateTime(2019, 1, 5, 14, 15, 33));

            var handler = new GetReplyToMessageQueryHandler(builder.Context);
            var query = new GetReplyToMessageQuery("sender@test.com", 1);

            Action action = () => handler.Execute(query);

            action.Should().Throw<InvalidOperationException>();
        }
    }
}