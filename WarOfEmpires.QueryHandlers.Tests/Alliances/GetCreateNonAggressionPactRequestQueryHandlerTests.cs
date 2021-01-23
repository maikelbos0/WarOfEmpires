using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Alliances {
    [TestClass]
    public sealed class GetCreateNonAggressionPactRequestQueryHandlerTests {
        [TestMethod]
        public void GetCreateNonAggressionPactRequestQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .BuildAlliance(1);

            var handler = new GetCreateNonAggressionPactRequestQueryHandler(builder.Context);
            var query = new GetCreateNonAggressionPactRequestQuery(1);

            var result = handler.Execute(query);

            result.AllianceId.Should().Be(1);
            result.AllianceCode.Should().Be("FS");
            result.AllianceName.Should().Be("Føroyskir Samgonga");
        }
    }
}
