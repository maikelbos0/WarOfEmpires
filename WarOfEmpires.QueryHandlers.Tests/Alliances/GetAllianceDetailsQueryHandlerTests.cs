﻿using FluentAssertions;
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
            var context = new FakeWarContext();
            var builder = new FakeBuilder(context).CreateAlliance(1);

            builder.CreateLeader(3, rank: 2).AddPopulation();
            builder.CreatePlayer(1, rank: 3);
            builder.CreatePlayer(2, status: UserStatus.Inactive);

            var handler = new GetAllianceDetailsQueryHandler(context, new EnumFormatter());
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