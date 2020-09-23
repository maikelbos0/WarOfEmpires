using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Alliances {
    [TestClass]
    public sealed class GetCurrentAllianceRightsQueryHandlerTests {
        [TestMethod]
        public void GetCurrentAllianceRightsQueryHandler_Succeeds_Not_In_Alliance() {
            var builder = new FakeBuilder()
                .WithPlayer(1);

            var handler = new GetCurrentAllianceRightsQueryHandler(builder.Context);
            var query = new GetCurrentAllianceRightsQuery("test1@test.com");

            var result = handler.Execute(query);

            result.IsInAlliance.Should().BeFalse();
            result.CanInvite.Should().BeFalse();
        }

        [TestMethod]
        public void GetCurrentAllianceRightsQueryHandler_Succeeds_In_Alliance() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1);

            var handler = new GetCurrentAllianceRightsQueryHandler(builder.Context);
            var query = new GetCurrentAllianceRightsQuery("test1@test.com");

            var result = handler.Execute(query);

            result.IsInAlliance.Should().BeTrue();
            result.CanInvite.Should().BeTrue();
        }
    }
}