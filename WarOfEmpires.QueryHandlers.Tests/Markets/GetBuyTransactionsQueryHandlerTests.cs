using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Queries.Markets;
using WarOfEmpires.QueryHandlers.Markets;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Tests.Markets {
    [TestClass]
    public sealed class GetBuyTransactionsQueryHandlerTests {
        [TestMethod]
        public void GetBuyTransactionsQueryHandler_Returns_All_Transactions() {
            var builder = new FakeBuilder().BuildPlayer(1)
                .WithBuyTransaction(MerchandiseType.Ore, 1000, 3)
                .WithBuyTransaction(MerchandiseType.Wood, 5000, 4)
                .WithBuyTransaction(MerchandiseType.Food, 2000, 2);

            var handler = new GetBuyTransactionsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetBuyTransactionsQuery("test1@test.com");

            var result = handler.Execute(query);

            result.Should().HaveCount(3);
        }

        [TestMethod]
        public void GetBuyTransactionsQueryHandler_Returns_Correct_Data() {
            var builder = new FakeBuilder().BuildPlayer(1)
                .WithBuyTransaction(MerchandiseType.Wood, 1234, 5);

            var handler = new GetBuyTransactionsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetBuyTransactionsQuery("test1@test.com");

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