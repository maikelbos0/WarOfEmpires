using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Alliances {
    [TestClass]
    public sealed class GetNewLeaderQueryHandlerTests {
        [TestMethod]
        public void GetNewLeaderQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithLeader(1)
                .WithMember(2)
                .WithMember(3, displayName: "Another test display name")
                .WithMember(4, status: UserStatus.Inactive);

            var handler = new GetNewLeaderQueryHandler(builder.Context);
            var query = new GetNewLeaderQuery("test1@test.com");

            var result = handler.Execute(query);

            result.Members.Should().HaveCount(2);

            result.Members.First().Id.Should().Be(3);
            result.Members.First().DisplayName.Should().Be("Another test display name");
        }
    }
}
