using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Alliances {
    [TestClass]
    public sealed class GetTransferResourcesQueryHandlerTests {
        [TestMethod]
        public void GetTransferResourcesQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1)
                .WithLeader(2, displayName: "Test name")
                .WithMember(3, displayName: "Another test display name")
                .WithMember(4, status: UserStatus.Inactive);

            var handler = new GetTransferResourcesQueryHandler(builder.Context);
            var query = new GetTransferResourcesQuery("test1@test.com");

            var result = handler.Execute(query);

            result.Recipients.Should().HaveCount(2);
            result.Recipients.Should().Contain(r => r.Id == 2).Subject.DisplayName.Should().Be("Test name");
            result.Recipients.Should().Contain(r => r.Id == 3).Subject.DisplayName.Should().Be("Another test display name");
        }
    }
}
