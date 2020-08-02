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
    public sealed class GetReceivedMessagesQueryHandlerTests {
        [TestMethod]
        public void GetReceivedMessagesQueryHandler_Returns_All_Received_Messages() {
            var builder = new FakeBuilder().BuildPlayer(1);

            builder.BuildPlayer(2).WithMessageTo(1, builder.Player, new DateTime(2019, 1, 1))
                .BuildPlayer(3).WithMessageTo(2, builder.Player, new DateTime(2020, 2, 1))
                .BuildPlayer(4, status: UserStatus.Inactive).WithMessageTo(3, builder.Player, new DateTime(2020, 3, 1));

            var handler = new GetReceivedMessagesQueryHandler(builder.Context);
            var query = new GetReceivedMessagesQuery("test1@test.com");

            var result = handler.Execute(query);

            result.Count().Should().Be(3);
        }

        [TestMethod]
        public void GetReceivedMessagesQueryHandler_Returns_Correct_Message_Data() {
            var builder = new FakeBuilder().BuildPlayer(1);

            builder.BuildPlayer(2, displayName: "Sender test").WithMessageTo(1, builder.Player, new DateTime(2019, 1, 1));

            var handler = new GetReceivedMessagesQueryHandler(builder.Context);
            var query = new GetReceivedMessagesQuery("test1@test.com");

            var result = handler.Execute(query);
            var message = result.FirstOrDefault();

            message.Should().NotBeNull();
            message.Sender.Should().Be("Sender test");
            message.Date.Should().Be(new DateTime(2019, 1, 1));
            message.IsRead.Should().BeFalse();
            message.Subject.Should().Be("Message subject");
        }
    }
}