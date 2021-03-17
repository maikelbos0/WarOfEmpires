﻿using FluentAssertions;
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
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithLeader(1)
                .BuildAlliance(2)
                .WithLeader(2);

            var handler = new GetAlliancesQueryHandler(builder.Context);
            var query = new GetAlliancesQuery(null, null);

            var result = handler.Execute(query);

            result.Should().HaveCount(2);
        }

        [TestMethod]
        public void GetAlliancesQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .BuildAlliance(2)
                .WithMember(1)
                .WithMember(2)
                .WithMember(3)
                .WithLeader(4, displayName: "Capitain");

            var handler = new GetAlliancesQueryHandler(builder.Context);
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
        public void GetAlliancesQueryHandler_Returns_Correct_Status_For_Own_Alliance() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void GetAlliancesQueryHandler_Returns_Correct_Status_For_Pact() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void GetAlliancesQueryHandler_Searches_By_Code() {
            var builder = new FakeBuilder()
                .BuildAlliance(1, "C1", "N1")
                .WithLeader(1)
                .BuildAlliance(2, "Cod2", "N2")
                .WithLeader(2)
                .BuildAlliance(3, "Cod3", "N3")
                .WithLeader(3)
                .BuildAlliance(4, "C4", "N4").BuildLeader(4);
            
            var handler = new GetAlliancesQueryHandler(builder.Context);
            var query = new GetAlliancesQuery("od", null);

            var result = handler.Execute(query);

            result.Should().HaveCount(2);
        }

        [TestMethod]
        public void GetAlliancesQueryHandler_Searches_By_Name() {
            var builder = new FakeBuilder()
                .BuildAlliance(1, "C1", "Name1")
                .WithLeader(1)
                .BuildAlliance(2, "C2", "N2")
                .WithLeader(2)
                .BuildAlliance(3, "C3", "Name3")
                .WithLeader(3)
                .BuildAlliance(4, "C4", "Name4")
                .WithLeader(4);

            var handler = new GetAlliancesQueryHandler(builder.Context);
            var query = new GetAlliancesQuery(null, "ame");

            var result = handler.Execute(query);

            result.Should().HaveCount(3);
        }
    }
}