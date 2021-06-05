using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Players {
    [TestClass]
    public sealed class GetCurrentPlayerQueryHandlerTests {
        [TestMethod]
        public void GetCurrentPlayerQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            builder.User.IsAdmin.Returns(false);

            var handler = new GetCurrentPlayerQueryHandler(builder.Context);
            var query = new GetCurrentPlayerQuery("test1@test.com");

            var result = handler.Execute(query);

            result.IsAuthenticated.Should().BeTrue();
            result.IsAdmin.Should().BeFalse();
            result.IsInAlliance.Should().BeFalse();
            result.CanTransferLeadership.Should().BeFalse();
            result.CanInvite.Should().BeFalse();
            result.CanManageRoles.Should().BeFalse();
            result.CanLeaveAlliance.Should().BeFalse();
            result.CanDisbandAlliance.Should().BeFalse();
            result.CanManageNonAggressionPacts.Should().BeFalse();
            result.CanManageWars.Should().BeFalse();
            result.DisplayName.Should().Be("Test display name 1");
        }

        [TestMethod]
        public void GetCurrentPlayerQueryHandler_Returns_Correct_Information_IsAdmin_IsInAlliance() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .BuildMember(1); 
            
            builder.User.IsAdmin.Returns(true);

            var handler = new GetCurrentPlayerQueryHandler(builder.Context);
            var query = new GetCurrentPlayerQuery("test1@test.com");

            var result = handler.Execute(query);

            result.IsAdmin.Should().BeTrue();
            result.IsInAlliance.Should().BeTrue();
            result.CanTransferLeadership.Should().BeFalse();
            result.CanInvite.Should().BeFalse();
            result.CanManageRoles.Should().BeFalse();
            result.CanLeaveAlliance.Should().BeTrue();
            result.CanDisbandAlliance.Should().BeFalse();
            result.CanManageNonAggressionPacts.Should().BeFalse();
            result.CanManageWars.Should().BeFalse();
        }

        [TestMethod]
        public void GetCurrentPlayerQueryHandler_Returns_Correct_Information_Rights_In_Role() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1, out var player)
                .WithRole(2, "Test", player);

            var handler = new GetCurrentPlayerQueryHandler(builder.Context);
            var query = new GetCurrentPlayerQuery("test1@test.com");

            var result = handler.Execute(query);

            result.CanInvite.Should().BeTrue();
            result.CanManageRoles.Should().BeTrue();
            result.CanLeaveAlliance.Should().BeTrue();
            result.CanTransferLeadership.Should().BeFalse();
            result.CanDisbandAlliance.Should().BeFalse();
            result.CanManageNonAggressionPacts.Should().BeTrue();
            result.CanManageWars.Should().BeTrue();
        }

        [TestMethod]
        public void GetCurrentPlayerQueryHandler_Returns_Correct_Information_Rights_For_Alliance_Leader() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithLeader(1);

            var handler = new GetCurrentPlayerQueryHandler(builder.Context);
            var query = new GetCurrentPlayerQuery("test1@test.com");

            var result = handler.Execute(query);

            result.CanInvite.Should().BeTrue();
            result.CanManageRoles.Should().BeTrue();
            result.CanLeaveAlliance.Should().BeFalse();
            result.CanTransferLeadership.Should().BeTrue();
            result.CanDisbandAlliance.Should().BeTrue();
            result.CanManageNonAggressionPacts.Should().BeTrue();
            result.CanManageWars.Should().BeTrue();
        }

        [TestMethod]
        public void GetCurrentPlayerQueryHandler_Throws_Exception_For_Nonexistent_Email() {
            var builder = new FakeBuilder()
                .BuildPlayer(1);

            var handler = new GetCurrentPlayerQueryHandler(builder.Context);
            var query = new GetCurrentPlayerQuery("wrong@test.com");

            Action action = () => handler.Execute(query);

            action.Should().Throw<InvalidOperationException>();
        }
    }
}