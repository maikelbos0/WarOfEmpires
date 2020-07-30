using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Alliances {
    [TestClass]
    public sealed class GetInvitePlayerQueryHandlerTests {
        [TestMethod]
        public void GetInvitePlayerQueryHandler_Returns_Correct_Information() {
            var context = new FakeWarContext();
            
            new FakeBuilder(context).CreatePlayer(1);

            var handler = new GetInvitePlayerQueryHandler(context);
            var query = new GetInvitePlayerQuery("1");

            var result = handler.Execute(query);

            result.PlayerId.Should().Be("1");
            result.PlayerName.Should().Be("Test display name 1");
        }
    }
}