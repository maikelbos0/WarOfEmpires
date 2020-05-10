using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Linq;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Alliances {
    [TestClass]
    public sealed class GetAlliancesQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        private void AddAlliance(int id, bool isActive, string code, string name, int members, string leader) {
            var alliance = Substitute.For<Alliance>();
            var players = Enumerable.Range(1, members).Select(n => Substitute.For<Player>()).ToList();

            alliance.Id.Returns(id);
            alliance.IsActive.Returns(isActive);
            alliance.Code.Returns(code);
            alliance.Name.Returns(name);
            alliance.Members.Returns(players);
            alliance.Leader.Returns(players.First());

            players.First().DisplayName.Returns(leader);

            _context.Alliances.Add(alliance);
        }

        [TestMethod]
        public void GetAlliancesQueryHandler_Returns_All_Active_Alliances() {
            AddAlliance(1, false, "c1", "n1", 3, null);
            AddAlliance(2, true, "c2", "n2", 4, null);
            AddAlliance(3, true, "c3", "n3", 5, null);

            var handler = new GetAlliancesQueryHandler(_context);
            var query = new GetAlliancesQuery(null, null);

            var result = handler.Execute(query);

            result.Should().HaveCount(2);
        }

        [TestMethod]
        public void GetAlliancesQueryHandler_Returns_Correct_Information() {
            AddAlliance(2, true, "c2", "n2", 4, "Capitain");

            var handler = new GetAlliancesQueryHandler(_context);
            var query = new GetAlliancesQuery(null, null);

            var result = handler.Execute(query);

            result.Should().HaveCount(1);
            result.Single().Id.Should().Be(2);
            result.Single().Code.Should().Be("c2");
            result.Single().Name.Should().Be("n2");
            result.Single().Members.Should().Be(4);
            result.Single().Leader.Should().Be("Capitain");
        }

        [TestMethod]
        public void GetAlliancesQueryHandler_Searches_By_Code() {
            AddAlliance(1, true, "c1", "n1", 3, null);
            AddAlliance(2, true, "cod2", "n2", 4, null);
            AddAlliance(3, true, "cod3", "n3", 5, null);
            AddAlliance(4, true, "c4", "n4", 6, null);

            var handler = new GetAlliancesQueryHandler(_context);
            var query = new GetAlliancesQuery("od", null);

            var result = handler.Execute(query);

            result.Should().HaveCount(2);
        }

        [TestMethod]
        public void GetAlliancesQueryHandler_Searches_By_Name() {
            AddAlliance(1, true, "c1", "name1", 3, null);
            AddAlliance(2, true, "c2", "n", 4, null);
            AddAlliance(3, true, "c3", "name3", 5, null);
            AddAlliance(4, true, "c4", "name4", 6, null);

            var handler = new GetAlliancesQueryHandler(_context);
            var query = new GetAlliancesQuery(null, "ame");

            var result = handler.Execute(query);

            result.Should().HaveCount(3);
        }
    }
}