﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Alliances {
    [TestClass]
    public sealed class GetSentNonAggressionPactRequestsQueryHandlerTests {
        [TestMethod]
        public void GetSentNonAggressionPactRequestsQueryHandler_Returns_All_Requests() {
            var builder = new FakeBuilder()
                .WithAlliance(2, out var firstAlliance, "TEST", "Pact test")
                .WithAlliance(3, out var secondAlliance, "ASDF", "Another test")
                .BuildAlliance(1)
                .WithLeader(1)
                .WithNonAggressionPactRequestTo(2, secondAlliance)
                .WithNonAggressionPactRequestTo(1, firstAlliance);

            var handler = new GetSentNonAggressionPactRequestsQueryHandler(builder.Context);
            var query = new GetSentNonAggressionPactRequestsQuery("test1@test.com");

            var result = handler.Execute(query);

            result.Should().HaveCount(2);
        }

        [TestMethod]
        public void GetSentNonAggressionPactRequestsQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .WithAlliance(2, out var firstAlliance, "TEST", "Pact test")
                .WithAlliance(3, out var secondAlliance, "ASDF", "Another test")
                .BuildAlliance(1)
                .WithLeader(1)
                .WithNonAggressionPactRequestTo(2, secondAlliance)
                .WithNonAggressionPactRequestTo(1, firstAlliance);

            var handler = new GetSentNonAggressionPactRequestsQueryHandler(builder.Context);
            var query = new GetSentNonAggressionPactRequestsQuery("test1@test.com");

            var result = handler.Execute(query);

            result.First().Id.Should().Be(1);
            result.First().AllianceId.Should().Be(2);
            result.First().Code.Should().Be("TEST");
            result.First().Name.Should().Be("Pact test");
        }
    }
}
