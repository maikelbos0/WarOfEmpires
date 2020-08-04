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
                .BuildAlliance(1)
                .WithMember(1, rank: 3)
                .WithMember(2, status: UserStatus.Inactive)
                .BuildLeader(3, rank: 2)
                .WithPopulation();

            var handler = new GetAllianceDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetAllianceDetailsQuery("1");

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
        }

        [TestMethod]
        public void GetAllianceDetailsQueryHandler_Throws_Exception_For_Alphanumeric_Id() {
            var handler = new GetAllianceDetailsQueryHandler(new FakeWarContext(), new EnumFormatter());
            var query = new GetAllianceDetailsQuery("A");

            Action action = () => handler.Execute(query);

            action.Should().Throw<FormatException>();
        }

        [TestMethod]
        public void GetAllianceDetailsQueryHandler_Throws_Exception_For_Nonexistent_Id() {
            var handler = new GetAllianceDetailsQueryHandler(new FakeWarContext(), new EnumFormatter());
            var query = new GetAllianceDetailsQuery("5");

            Action action = () => handler.Execute(query);

            action.Should().Throw<InvalidOperationException>();
        }
    }
}