using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Players;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Tests.Players {
    [TestClass]
    public sealed class GetPlayerDetailsQueryHandlerTests {
        [TestMethod]
        public void GetPlayerDetailsQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .BuildAlliance(14)
                .BuildMember(2, rank: 1)
                .WithPopulation();

            var handler = new GetPlayerDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetPlayerDetailsQuery("test1@test.com", 2);

            var result = handler.Execute(query);

            result.Id.Should().Be(2);
            result.Rank.Should().Be(1);
            result.Title.Should().Be("Sub chieftain");
            result.DisplayName.Should().Be("Test display name 2");
            result.Population.Should().Be(49);
            result.AllianceId.Should().Be(14);
            result.AllianceCode.Should().Be("FS");
            result.AllianceName.Should().Be("Føroyskir Samgonga");
            result.CanBeAttacked.Should().BeTrue();
        }

        [TestMethod]
        public void GetPlayerDetailsQueryHandler_Returns_CanBeAttacked_True_For_Allianceless() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .WithPlayer(2);

            var handler = new GetPlayerDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetPlayerDetailsQuery("test1@test.com", 2);

            var result = handler.Execute(query);

            result.CanBeAttacked.Should().BeTrue();
        }

        [TestMethod]
        public void GetPlayerDetailsQueryHandler_Returns_CanBeAttacked_False_For_Self() {
            var builder = new FakeBuilder()
                .WithPlayer(1);

            var handler = new GetPlayerDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetPlayerDetailsQuery("test1@test.com", 1);

            var result = handler.Execute(query);
            
            result.CanBeAttacked.Should().BeFalse();
        }

        [TestMethod]
        public void GetPlayerDetailsQueryHandler_Returns_CanBeAttacked_False_For_Alliance_Member() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithLeader(1)
                .WithMember(2);

            var handler = new GetPlayerDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetPlayerDetailsQuery("test1@test.com", 2);

            var result = handler.Execute(query);

            result.CanBeAttacked.Should().BeFalse();
        }

        [TestMethod]
        public void GetPlayerDetailsQueryHandler_Returns_CanBeAttacked_False_For_Pact() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithLeader(1);

            builder.BuildAlliance(2)
                .WithMember(2)
                .WithNonAggressionPact(1, builder.Alliance);

            var handler = new GetPlayerDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetPlayerDetailsQuery("test1@test.com", 2);

            var result = handler.Execute(query);

            result.CanBeAttacked.Should().BeFalse();
        }

        [TestMethod]
        public void GetPlayerDetailsQueryHandler_Throws_Exception_For_Nonexistent_Id() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new GetPlayerDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetPlayerDetailsQuery("test1@test.com", 5);

            Action action = () => handler.Execute(query);

            action.Should().Throw<InvalidOperationException>();
        }
    }
}