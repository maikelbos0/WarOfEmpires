using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Alliances {
    [TestClass]
    public sealed class GetAllianceNameQueryHandlerTests {        
        [TestMethod]
        public void GetAllianceNameQueryHandler_Returns_Correct_Name() {
            var builder = new FakeBuilder().CreateAlliance(1);

            builder.CreateMember(1);

            var handler = new GetAllianceNameQueryHandler(builder.Context);
            var query = new GetAllianceNameQuery("test1@test.com");

            var result = handler.Execute(query);

            result.Should().Be("Føroyskir Samgonga");
        }
    }
}
