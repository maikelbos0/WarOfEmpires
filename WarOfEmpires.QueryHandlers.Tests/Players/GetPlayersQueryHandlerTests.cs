using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Players;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Tests.Players {
    [TestClass]
    public sealed class GetPlayersQueryHandlerTests {
        [TestMethod]
        public void GetPlayersQueryHandler_Returns_All_Active_Players() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .BuildPlayer(2, status: UserStatus.Inactive)
                .BuildPlayer(3, status: UserStatus.New)
                .BuildPlayer(4)
                .BuildPlayer(5);

            var handler = new GetPlayersQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetPlayersQuery(null);

            var result = handler.Execute(query);

            result.Should().HaveCount(3);
        }

        [TestMethod]
        public void GetPlayersQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder().BuildAlliance(1)
                .BuildMember(1, rank: 5)
                .WithPopulation();

            var handler = new GetPlayersQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetPlayersQuery(null);

            var result = handler.Execute(query);

            result.Should().HaveCount(1);
            result.Single().Id.Should().Be(1);
            result.Single().Rank.Should().Be(5);
            result.Single().Title.Should().Be("Sub chieftain");
            result.Single().DisplayName.Should().Be("Test display name 1");
            result.Single().Alliance.Should().Be("FS");
            result.Single().Population.Should().Be(49);
        }

        [TestMethod]
        public void GetPlayersQueryHandler_Searches() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .BuildPlayer(2, displayName: "Wrong");

            var handler = new GetPlayersQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetPlayersQuery("Test");

            var result = handler.Execute(query);

            result.Should().HaveCount(1);
            result.Single().DisplayName.Should().Be("Test display name 1");
        }
    }
}