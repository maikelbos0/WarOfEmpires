using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Empires;
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
                .BuildMember(2, rank: 1, creationDate: DateTime.UtcNow.AddHours(-24).AddMinutes(-1))
                .WithBuilding(BuildingType.Defences, 4)
                .WithPopulation();

            var handler = new GetPlayerDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetPlayerDetailsQuery("test1@test.com", 2);

            var result = handler.Execute(query);

            result.Id.Should().Be(2);
            result.Rank.Should().Be(1);
            result.Title.Should().Be("Sub chieftain");
            result.DisplayName.Should().Be("Test display name 2");
            result.Population.Should().Be(49);
            result.Defences.Should().Be("Walled fort");
            result.AllianceId.Should().Be(14);
            result.AllianceCode.Should().Be("FS");
            result.AllianceName.Should().Be("Føroyskir Samgonga");
            result.CanBeAttacked.Should().BeTrue();
            result.GrandOverlordTime.Should().BeNull();
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
        public void GetPlayerDetailsQueryHandler_Returns_CanBeAttacked_False_For_New_Player() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .WithPlayer(2, creationDate: DateTime.UtcNow.AddHours(-23).AddMinutes(-59));

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
        public void GetPlayerDetailsQueryHandler_Returns_Empty_Status_By_Default() {
            var builder = new FakeBuilder()
                .WithPlayer(2)
                .WithPlayer(1);

            var handler = new GetPlayerDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetPlayerDetailsQuery("test1@test.com", 2);

            var result = handler.Execute(query);

            result.Status.Should().BeNull();
        }

        [TestMethod]
        public void GetPlayerDetailsQueryHandler_Returns_Correct_Status_For_Self() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1);

            var handler = new GetPlayerDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetPlayerDetailsQuery("test1@test.com", 1);

            var result = handler.Execute(query);

            result.Status.Should().Be("Mine");
        }

        [TestMethod]
        public void GetPlayerDetailsQueryHandler_Returns_Correct_Status_For_Own_Alliance() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1)
                .WithMember(2, displayName: "Allied");

            var handler = new GetPlayerDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetPlayerDetailsQuery("test1@test.com", 2);

            var result = handler.Execute(query);

            result.Status.Should().Be("Ally");
        }

        [TestMethod]
        public void GetPlayerDetailsQueryHandler_Returns_Correct_Status_For_Pact() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1);

            builder.BuildAlliance(2)
                .WithMember(2, displayName: "Don't attack")
                .WithWar(1, builder.Alliance)
                .WithNonAggressionPact(1, builder.Alliance);

            var handler = new GetPlayerDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetPlayerDetailsQuery("test1@test.com", 2);

            var result = handler.Execute(query);

            result.Status.Should().Be("Pact");
        }

        [TestMethod]
        public void GetPlayerDetailsQueryHandler_Returns_Correct_Status_For_War() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1);

            builder.BuildAlliance(2)
                .WithMember(2, displayName: "Don't attack")
                .WithWar(1, builder.Alliance);

            var handler = new GetPlayerDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetPlayerDetailsQuery("test1@test.com", 2);

            var result = handler.Execute(query);

            result.Status.Should().Be("War");
        }

        [TestMethod]
        public void GetPlayerDetailsQueryHandler_Returns_GrandOverlordTime_If_Not_Zero() {
            var builder = new FakeBuilder()
                .BuildPlayer(1, grandOverlordTime: TimeSpan.FromMinutes(1234));

            var handler = new GetPlayerDetailsQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetPlayerDetailsQuery("test1@test.com", 1);

            var result = handler.Execute(query);

            result.GrandOverlordTime.Should().Be(TimeSpan.FromMinutes(1234));
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