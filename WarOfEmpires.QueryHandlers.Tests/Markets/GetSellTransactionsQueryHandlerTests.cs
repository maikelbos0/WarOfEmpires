using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Markets;
using WarOfEmpires.QueryHandlers.Markets;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Tests.Markets {
    [TestClass]
    public sealed class GetSellTransactionsQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly EnumFormatter _formatter = new EnumFormatter();

        public Player AddPlayer(int id, string email, string displayName, UserStatus status) {
            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();

            user.Id.Returns(id);
            user.Status.Returns(status);
            user.Email.Returns(email);

            player.User.Returns(user);
            player.Id.Returns(id);
            player.DisplayName.Returns(displayName);

            _context.Users.Add(user);
            _context.Players.Add(player);

            return player;
        }

        [TestMethod]
        public void GetSellTransactionsQueryHandler_Returns_All_Transactions() {
            var handler = new GetSellTransactionsQueryHandler(_context, _formatter);
            var query = new GetSellTransactionsQuery("test@test.com");
            var seller = AddPlayer(1, "test@test.com", "Test", UserStatus.Active);
            var buyers = new[] {
                AddPlayer(2, "active1@test.com", "Test", UserStatus.Active),
                AddPlayer(3, "active2@test.com", "Test", UserStatus.Active),
                AddPlayer(4, "inactive@test.com", "Test", UserStatus.Inactive),
            };

            seller.SellTransactions.Returns(buyers.Select(s => new Transaction(MerchandiseType.Ore, 1000, 3)).ToList());

            var result = handler.Execute(query);

            result.Should().HaveCount(3);
        }

        [TestMethod]
        public void GetSellTransactionsQueryHandler_Returns_Correct_Data() {
            var handler = new GetSellTransactionsQueryHandler(_context, _formatter);
            var query = new GetSellTransactionsQuery("test@test.com");
            var seller = AddPlayer(1, "test@test.com", "Recipient", UserStatus.Active);

            seller.SellTransactions.Returns(new List<Transaction>() { new Transaction(MerchandiseType.Wood, 1234, 5) });

            var result = handler.Execute(query);
            var transaction = result.FirstOrDefault();

            transaction.Should().NotBeNull();
            transaction.Date.Should().BeCloseTo(DateTime.UtcNow, 1000);
            transaction.Type.Should().Be("Wood");
            transaction.Quantity.Should().Be(1234);
            transaction.Price.Should().Be(5);
        }
    }
}