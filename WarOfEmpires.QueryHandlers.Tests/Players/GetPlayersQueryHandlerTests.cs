﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
                .WithPlayer(1)
                .WithPlayer(2, status: UserStatus.Inactive)
                .WithPlayer(3, status: UserStatus.New)
                .WithPlayer(4)
                .WithPlayer(5);

            var handler = new GetPlayersQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetPlayersQuery("test1@test.com", null);

            var result = handler.Execute(query);

            result.Should().HaveCount(3);
        }

        [TestMethod]
        public void GetPlayersQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .BuildMember(1, rank: 5, grandOverlordTime: TimeSpan.FromMinutes(1234))
                .WithPopulation();

            var handler = new GetPlayersQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetPlayersQuery("test1@test.com", null);

            var result = handler.Execute(query);

            result.Should().HaveCount(1);
            result.Single().Id.Should().Be(1);
            result.Single().Rank.Should().Be(5);
            result.Single().Title.Should().Be("Sub chieftain");
            result.Single().DisplayName.Should().Be("Test display name 1");
            result.Single().Race.Should().Be("Elves");
            result.Single().Alliance.Should().Be("FS");
            result.Single().Population.Should().Be(49);
            result.Single().GrandOverlordTime.Should().Be(TimeSpan.FromMinutes(1234));
        }

        [TestMethod]
        public void GetPlayersQueryHandler_Returns_Empty_Status_By_Default() {
            var builder = new FakeBuilder()
                .WithPlayer(2, displayName: "Player", creationDate: DateTime.UtcNow.AddHours(-24).AddMinutes(-1))
                .WithPlayer(1);

            var handler = new GetPlayersQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetPlayersQuery("test1@test.com", "Player");

            var result = handler.Execute(query);

            result.Should().HaveCount(1);
            result.Single().Status.Should().BeNull();
        }

        [TestMethod]
        public void GetPlayersQueryHandler_Returns_Correct_Status_For_Self() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1);

            var handler = new GetPlayersQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetPlayersQuery("test1@test.com", null);

            var result = handler.Execute(query);

            result.Should().HaveCount(1);
            result.Single().Status.Should().Be("Mine");
        }

        [TestMethod]
        public void GetPlayersQueryHandler_Returns_Correct_Status_For_New() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .WithPlayer(2, displayName: "New", creationDate: DateTime.UtcNow.AddHours(-23).AddMinutes(-59));

            var handler = new GetPlayersQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetPlayersQuery("test1@test.com", "New");

            var result = handler.Execute(query);

            result.Should().HaveCount(1);
            result.Single().Status.Should().Be("New");
        }

        [TestMethod]
        public void GetPlayersQueryHandler_Returns_Correct_Status_For_Own_Alliance() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1)
                .WithMember(2, displayName: "Allied");

            var handler = new GetPlayersQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetPlayersQuery("test1@test.com", "Allied");

            var result = handler.Execute(query);

            result.Should().HaveCount(1);
            result.Single().Status.Should().Be("Ally");
        }

        [TestMethod]
        public void GetPlayersQueryHandler_Returns_Correct_Status_For_Pact() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1);

            builder.BuildAlliance(2)
                .WithMember(2, displayName: "Don't attack")
                .WithWar(1, builder.Alliance)
                .WithNonAggressionPact(1, builder.Alliance);

            var handler = new GetPlayersQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetPlayersQuery("test1@test.com", "Don't attack");

            var result = handler.Execute(query);

            result.Should().HaveCount(1);
            result.Single().Status.Should().Be("Pact");
        }

        [TestMethod]
        public void GetPlayersQueryHandler_Returns_Correct_Status_For_War() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1);

            builder.BuildAlliance(2)
                .WithMember(2, displayName: "Attack!")
                .WithWar(1, builder.Alliance);

            var handler = new GetPlayersQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetPlayersQuery("test1@test.com", "Attack!");

            var result = handler.Execute(query);

            result.Should().HaveCount(1);
            result.Single().Status.Should().Be("War");
        }

        [TestMethod]
        public void GetPlayersQueryHandler_Searches() {
            var builder = new FakeBuilder()
                .WithPlayer(1)
                .WithPlayer(2, displayName: "Wrong");

            var handler = new GetPlayersQueryHandler(builder.Context, new EnumFormatter());
            var query = new GetPlayersQuery("test1@test.com", "Test");

            var result = handler.Execute(query);

            result.Should().HaveCount(1);
            result.Single().DisplayName.Should().Be("Test display name 1");
        }
    }
}