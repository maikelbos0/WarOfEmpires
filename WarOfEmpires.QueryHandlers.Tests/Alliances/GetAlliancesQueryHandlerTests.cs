using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Alliances {
    [TestClass]
    public sealed class GetAlliancesQueryHandlerTests {
        [TestMethod]
        public void GetAlliancesQueryHandler_Returns_All_Alliances() {
            var context = new FakeWarContext();
            var builder = new FakeBuilder(context);

            builder.CreateAlliance(1).CreateLeader(1);
            builder.CreateAlliance(2).CreateLeader(2);

            var handler = new GetAlliancesQueryHandler(context);
            var query = new GetAlliancesQuery(null, null);

            var result = handler.Execute(query);

            result.Should().HaveCount(2);
        }

        [TestMethod]
        public void GetAlliancesQueryHandler_Returns_Correct_Information() {
            var context = new FakeWarContext();
            var builder = new FakeBuilder(context).CreateAlliance(2);

            builder.CreatePlayer(1);
            builder.CreatePlayer(2);
            builder.CreatePlayer(3);
            builder.CreateLeader(4, displayName: "Capitain");

            var handler = new GetAlliancesQueryHandler(context);
            var query = new GetAlliancesQuery(null, null);

            var result = handler.Execute(query);

            result.Should().HaveCount(1);
            result.Single().Id.Should().Be(2);
            result.Single().Code.Should().Be("FS");
            result.Single().Name.Should().Be("Føroyskir Samgonga");
            result.Single().Members.Should().Be(4);
            result.Single().Leader.Should().Be("Capitain");
        }

        [TestMethod]
        public void GetAlliancesQueryHandler_Searches_By_Code() {
            var context = new FakeWarContext();
            var builder = new FakeBuilder(context);

            builder.CreateAlliance(1, "C1", "N1").CreateLeader(1);
            builder.CreateAlliance(2, "Cod2", "N2").CreateLeader(2);
            builder.CreateAlliance(3, "Cod3", "N3").CreateLeader(3);
            builder.CreateAlliance(4, "C4", "N4").CreateLeader(4);
            
            var handler = new GetAlliancesQueryHandler(context);
            var query = new GetAlliancesQuery("od", null);

            var result = handler.Execute(query);

            result.Should().HaveCount(2);
        }

        [TestMethod]
        public void GetAlliancesQueryHandler_Searches_By_Name() {
            var context = new FakeWarContext();
            var builder = new FakeBuilder(context);

            builder.CreateAlliance(1, "C1", "Name1").CreateLeader(1);
            builder.CreateAlliance(2, "C2", "N2").CreateLeader(2);
            builder.CreateAlliance(3, "C3", "Name3").CreateLeader(3);
            builder.CreateAlliance(4, "C4", "Name4").CreateLeader(4);

            var handler = new GetAlliancesQueryHandler(context);
            var query = new GetAlliancesQuery(null, "ame");

            var result = handler.Execute(query);

            result.Should().HaveCount(3);
        }
    }
}