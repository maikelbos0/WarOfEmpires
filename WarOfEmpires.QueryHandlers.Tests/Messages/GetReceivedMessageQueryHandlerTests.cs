﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.QueryHandlers.Messages;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Messages {
    [TestClass]
    public sealed class GetReceivedMessageQueryHandlerTests {
        [TestMethod]
        public void GetReceivedMessageQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var player)
                .BuildPlayer(2, email: "sender@test.com", displayName: "Sender test")
                .WithMessageTo(1, player, new DateTime(2019, 1, 1));

            var handler = new GetReceivedMessageQueryHandler(builder.Context);
            var query = new GetReceivedMessageQuery("test1@test.com", 1);

            var result = handler.Execute(query);

            result.Subject.Should().Be("Message subject");
            result.Body.Should().Be("Message body");
            result.SenderId.Should().Be(2);
            result.Sender.Should().Be("Sender test");
            result.Date.Should().Be(new DateTime(2019, 1, 1));
            result.IsRead.Should().BeFalse();
        }

        [TestMethod]
        public void GetReceivedMessageQueryHandler_Handles_Inactive_Sender() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var player)
                .BuildPlayer(2, email: "sender@test.com", displayName: "Sender test", status: UserStatus.Inactive)
                .WithMessageTo(1, player, new DateTime(2019, 1, 1));

            var handler = new GetReceivedMessageQueryHandler(builder.Context);
            var query = new GetReceivedMessageQuery("test1@test.com", 1);

            var result = handler.Execute(query);

            result.SenderId.Should().BeNull();
            result.Sender.Should().Be("Sender test");
        }

        [TestMethod]
        public void GetReceivedMessageQueryHandler_Throws_Exception_For_Message_Received_By_Different_Player() {
            var builder = new FakeBuilder()
                .WithPlayer(1, out var player)
                .BuildPlayer(2, email: "sender@test.com", displayName: "Sender test")
                .WithMessageTo(1, player, new DateTime(2019, 1, 1));

            var handler = new GetReceivedMessageQueryHandler(builder.Context);
            var query = new GetReceivedMessageQuery("sender@test.com", 1);

            Action action = () => handler.Execute(query);

            action.Should().Throw<InvalidOperationException>();
        }
    }
}