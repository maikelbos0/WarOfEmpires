using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.QueryHandlers.Messages;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Messages {
    [TestClass]
    public sealed class GetMessageRecipientQueryHandlerTests {
        [TestMethod]
        public void GetMessageRecipientQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .WithPlayer(2);

            var handler = new GetMessageRecipientQueryHandler(builder.Context);
            var query = new GetMessageRecipientQuery(2);

            var result = handler.Execute(query);

            result.RecipientId.Should().Be("2");
            result.Recipient.Should().Be("Test display name 2");
        }

        [TestMethod]
        public void GetMessageRecipientQueryHandler_Throws_Exception_For_Nonexistent_Id() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .WithPlayer(2);

            var handler = new GetMessageRecipientQueryHandler(builder.Context);
            var query = new GetMessageRecipientQuery(5);

            Action action = () => handler.Execute(query);

            action.Should().Throw<InvalidOperationException>();
        }
    }
}