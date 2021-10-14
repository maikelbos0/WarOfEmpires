using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Players {
    [TestClass]
    public sealed class GetPlayerIsCreatedQueryHandlerTests {
        [TestMethod]
        public void GetPlayerIsCreatedQueryHandler_Returns_True_For_Created() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new GetPlayerIsCreatedQueryHandler(builder.Context);
            var query = new GetPlayerIsCreatedQuery("test1@test.com");

            var result = handler.Execute(query);

            result.Should().BeTrue();
        }

        [TestMethod]
        public void GetPlayerIsCreatedQueryHandler_Returns_False_For_Not_Created() {
            var builder = new FakeBuilder()
                .BuildUser(1);

            var handler = new GetPlayerIsCreatedQueryHandler(builder.Context);
            var query = new GetPlayerIsCreatedQuery("test1@test.com");

            var result = handler.Execute(query);

            result.Should().BeFalse();
        }
    }
}
