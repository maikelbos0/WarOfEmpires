using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.QueryHandlers.Messages;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Messages {
    [TestClass]
    public sealed class GetMessageRecipientQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        public void AddPlayer(int id, string displayName) {
            var player = Substitute.For<Player>();

            player.Id.Returns(id);
            player.DisplayName.Returns(displayName);

            _context.Players.Add(player);
        }

        [TestMethod]
        public void GetMessageRecipientQueryHandler_Returns_Correct_Information() {
            var handler = new GetMessageRecipientQueryHandler(_context);
            var query = new GetMessageRecipientQuery("2");

            AddPlayer(1, "Test display name 1");
            AddPlayer(2, "Test display name 2");

            var results = handler.Execute(query);

            results.RecipientId.Should().Be("2");
            results.Recipient.Should().Be("Test display name 2");
        }

        [TestMethod]
        public void GetMessageRecipientQueryHandler_Throws_Exception_For_Alphanumeric_Id() {
            var handler = new GetMessageRecipientQueryHandler(_context);
            var query = new GetMessageRecipientQuery("A");

            AddPlayer(1, "Test display name 1");

            Action action = () => handler.Execute(query);

            action.Should().Throw<FormatException>();
        }

        [TestMethod]
        public void GetMessageRecipientQueryHandler_Throws_Exception_For_Nonexistent_Id() {
            var handler = new GetMessageRecipientQueryHandler(_context);
            var query = new GetMessageRecipientQuery("5");

            AddPlayer(1, "Test display name 1");

            Action action = () => handler.Execute(query);

            action.Should().Throw<InvalidOperationException>();
        }
    }
}