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
    public sealed class GetSellTransactionsQueryHandlerTests {
        [TestMethod]
        public void GetSellTransactionsQueryHandler_Returns_All_Transactions() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithSellTransaction(MerchandiseType.Ore, 1000, 3)
                .WithSellTransaction(MerchandiseType.Wood, 5000, 4)
                .WithSellTransaction(MerchandiseType.Food, 2000, 2);

            var handler = new GetSellTransactionsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetSellTransactionsQuery("test1@test.com");

            var result = handler.Execute(query);

            result.Should().HaveCount(3);
        }

        [TestMethod]
        public void GetSellTransactionsQueryHandler_Returns_Correct_Data() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithSellTransaction(MerchandiseType.Wood, 1234, 5);

            var handler = new GetSellTransactionsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetSellTransactionsQuery("test1@test.com");

            var result = handler.Execute(query);
            var transaction = result.FirstOrDefault();

            transaction.Should().NotBeNull();
            transaction.Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            transaction.Type.Should().Be("Wood");
            transaction.Quantity.Should().Be(1234);
            transaction.Price.Should().Be(5);
        }
    }
}