using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Alliances;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Tests.Alliances {
    [TestClass]
    public sealed class GetAllianceDetailsQueryHandlerTests {
        [TestMethod]
        public void GetAllianceDetailsQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .BuildAlliance(2)
                .WithLeader(1)
                .BuildAlliance(1)
                .WithMember(4, rank: 3)
                .WithMember(2, status: UserStatus.Inactive)
                .BuildLeader(3, rank: 2)
                .WithPopulation();

            var handler = new GetAllianceDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetAllianceDetailsQuery("test1@test.com", 1);

            var result = handler.Execute(query);

            result.Id.Should().Be(1);
            result.Code.Should().Be("FS");
            result.Name.Should().Be("Føroyskir Samgonga");
            result.LeaderId.Should().Be(3);
            result.Leader.Should().Be("Test display name 3");
            result.Members.Should().HaveCount(2);
            result.Members.First().Id.Should().Be(3);
            result.Members.First().Rank.Should().Be(2);
            result.Members.First().DisplayName.Should().Be("Test display name 3");
            result.Members.First().Title.Should().Be("Sub chieftain");
            result.Members.First().Population.Should().Be(49);
            result.CanReceiveNonAggressionPactRequest.Should().BeTrue();
            result.CanReceiveWarDeclaration.Should().BeTrue();
        }

        public void GetAllianceDetailsQueryHandler_CanReceiveNonAggressionPactRequest_Returns_False_For_Allianceless() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .BuildAlliance(1);

            var handler = new GetAllianceDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetAllianceDetailsQuery("test1@test.com", 1);

            var result = handler.Execute(query);

            result.CanReceiveNonAggressionPactRequest.Should().BeFalse();
        }

        public void GetAllianceDetailsQueryHandler_CanReceiveNonAggressionPactRequest_Returns_False_For_Self() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1);

            var handler = new GetAllianceDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetAllianceDetailsQuery("test1@test.com", 1);

            var result = handler.Execute(query);

            result.CanReceiveNonAggressionPactRequest.Should().BeFalse();
        }

        public void GetAllianceDetailsQueryHandler_CanReceiveNonAggressionPactRequest_Returns_False_For_Outstanding_Request() {
            var builder = new FakeBuilder()
                .WithAlliance(1, out var alliance)
                .BuildAlliance(2)
                .WithLeader(1)
                .WithNonAggressionPactRequestTo(1, alliance);

            var handler = new GetAllianceDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetAllianceDetailsQuery("test1@test.com", 1);

            var result = handler.Execute(query);

            result.CanReceiveNonAggressionPactRequest.Should().BeFalse();
        }

        public void GetAllianceDetailsQueryHandler_CanReceiveNonAggressionPactRequest_Returns_False_For_Existing_Pact() {
            var builder = new FakeBuilder()
                .WithAlliance(1, out var alliance)
                .BuildAlliance(2)
                .WithLeader(1)
                .WithNonAggressionPact(1, alliance);

            var handler = new GetAllianceDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetAllianceDetailsQuery("test1@test.com", 1);

            var result = handler.Execute(query);

            result.CanReceiveNonAggressionPactRequest.Should().BeFalse();
        }

        public void GetAllianceDetailsQueryHandler_CanReceiveWarDeclaration_Returns_False_For_Allianceless() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .BuildAlliance(1);

            var handler = new GetAllianceDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetAllianceDetailsQuery("test1@test.com", 1);

            var result = handler.Execute(query);

            result.CanReceiveWarDeclaration.Should().BeFalse();
        }

        public void GetAllianceDetailsQueryHandler_CanReceiveWarDeclaration_Returns_False_For_Self() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1);

            var handler = new GetAllianceDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetAllianceDetailsQuery("test1@test.com", 1);

            var result = handler.Execute(query);

            result.CanReceiveWarDeclaration.Should().BeFalse();
        }

        public void GetAllianceDetailsQueryHandler_CanReceiveWarDeclaration_Returns_False_For_Existing_War() {
            var builder = new FakeBuilder()
                .WithAlliance(1, out var alliance)
                .BuildAlliance(2)
                .WithLeader(1)
                .WithWar(1, alliance);

            var handler = new GetAllianceDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetAllianceDetailsQuery("test1@test.com", 1);

            var result = handler.Execute(query);

            result.CanReceiveWarDeclaration.Should().BeFalse();
        }

        [TestMethod]
        public void GetAllianceDetailsQueryHandler_Returns_Empty_Status_By_Default() {
            var builder = new FakeBuilder()
                .BuildAlliance(2)
                .WithLeader(2)
                .BuildAlliance(1)
                .WithLeader(1);

            var handler = new GetAllianceDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetAllianceDetailsQuery("test1@test.com", 2);

            var result = handler.Execute(query);

            result.Status.Should().BeNull();
        }

        [TestMethod]
        public void GetAllianceDetailsQueryHandler_Returns_Correct_Status_For_Own_Alliance() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithLeader(1);

            var handler = new GetAllianceDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetAllianceDetailsQuery("test1@test.com", 1);

            var result = handler.Execute(query);

            result.Status.Should().Be("Mine");
        }

        [TestMethod]
        public void GetAllianceDetailsQueryHandler_Returns_Correct_Status_For_Pact() {
            var builder = new FakeBuilder()
                .BuildAlliance(2)
                .WithLeader(2);

            builder.BuildAlliance(1)
                .WithLeader(1)
                .WithNonAggressionPact(1, builder.Alliance);

            var handler = new GetAllianceDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetAllianceDetailsQuery("test1@test.com", 2);

            var result = handler.Execute(query);

            result.Status.Should().Be("Pact");
        }

        [TestMethod]
        public void GetAllianceDetailsQueryHandler_Throws_Exception_For_Nonexistent_Id() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithLeader(1);

            var handler = new GetAllianceDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetAllianceDetailsQuery("test1@test.com", 5);

            Action action = () => handler.Execute(query);

            action.Should().Throw<InvalidOperationException>();
        }
    }
}