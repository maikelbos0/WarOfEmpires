using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WarOfEmpires.Queries.Markets;
using WarOfEmpires.QueryHandlers.Markets;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Tests.Markets {
    [TestClass]
    public sealed class GetBlackMarketQueryHandlerTests {
        [TestMethod]
        public void GetBlackMarketQueryHandler_Returns_Correct_Information() {
            var handler = new GetBlackMarketQueryHandler(new EnumFormatter());
            var query = new GetBlackMarketQuery();

            var result = handler.Execute(query);

            result.SellPrice.Should().Be(1);
            result.BuyPrice.Should().Be(20);
            result.Merchandise.Should().HaveCount(4);

            result.Merchandise.Single(m => m.Type == "Food").Name.Should().Be("Food");
            result.Merchandise.Single(m => m.Type == "Wood").Name.Should().Be("Wood");
            result.Merchandise.Single(m => m.Type == "Stone").Name.Should().Be("Stone");
            result.Merchandise.Single(m => m.Type == "Ore").Name.Should().Be("Ore");
        }
    }
}
