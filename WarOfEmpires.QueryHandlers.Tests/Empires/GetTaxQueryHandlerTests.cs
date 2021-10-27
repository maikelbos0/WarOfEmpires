using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Empires;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Empires {
    [TestClass]
    public sealed class GetTaxQueryHandlerTests {        
        [TestMethod]
        public void GetTaxQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            builder.Player.Tax.Returns(50);

            var query = new GetTaxQuery("test1@test.com");
            var handler = new GetTaxQueryHandler(builder.Context);

            var result = handler.Execute(query);

            result.Tax.Should().Be(50);

            result.BaseGoldPerTurn.Should().Be(500);
            result.BaseFoodPerTurn.Should().Be(22);
            result.BaseWoodPerTurn.Should().Be(26);
            result.BaseStonePerTurn.Should().Be(16);
            result.BaseOrePerTurn.Should().Be(16);

            result.CurrentGoldPerWorkerPerTurn.Should().Be(250);
            result.CurrentFoodPerWorkerPerTurn.Should().Be(11);
            result.CurrentWoodPerWorkerPerTurn.Should().Be(13);
            result.CurrentStonePerWorkerPerTurn.Should().Be(8);
            result.CurrentOrePerWorkerPerTurn.Should().Be(8);
        }
    }
}
