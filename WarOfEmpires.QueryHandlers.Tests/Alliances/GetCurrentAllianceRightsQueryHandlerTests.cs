using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Alliances {
    [TestClass]
    public sealed class GetCurrentAllianceRightsQueryHandlerTests {
        [TestMethod]
        public void GetCurrentAllianceRightsQueryHandler_Succeeds_Not_In_Alliance() {
            var builder = new FakeBuilder()
                .WithPlayer(1);

            var handler = new GetCurrentAllianceRightsQueryHandler(builder.Context);
            var query = new GetCurrentAllianceRightsQuery("test1@test.com");

            var result = handler.Execute(query);

            result.IsInAlliance.Should().BeFalse();
            result.CanInvite.Should().BeFalse();
            result.CanManageRoles.Should().BeFalse();
            result.CanDeleteChatMessages.Should().BeFalse();
            result.CanKickMembers.Should().BeFalse();
            result.CanTransferLeadership.Should().BeFalse();
            result.CanDisbandAlliance.Should().BeFalse();
            result.CanManageNonAggressionPacts.Should().BeFalse();
            result.CanManageWars.Should().BeFalse();
            result.CanBank.Should().BeFalse();
        }

        [TestMethod]
        public void GetCurrentAllianceRightsQueryHandler_Succeeds_In_Alliance() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1, out var player)
                .WithRole(2, "Test", player);

            var handler = new GetCurrentAllianceRightsQueryHandler(builder.Context);
            var query = new GetCurrentAllianceRightsQuery("test1@test.com");

            var result = handler.Execute(query);

            result.IsInAlliance.Should().BeTrue();
            result.CanInvite.Should().BeTrue();
            result.CanManageRoles.Should().BeTrue();
            result.CanDeleteChatMessages.Should().BeTrue();
            result.CanKickMembers.Should().BeTrue();
            result.CanTransferLeadership.Should().BeFalse();
            result.CanDisbandAlliance.Should().BeFalse();
            result.CanManageNonAggressionPacts.Should().BeTrue();
            result.CanManageWars.Should().BeTrue();
            result.CanBank.Should().BeTrue();
        }

        [TestMethod]
        public void GetCurrentAllianceRightsQueryHandler_Succeeds_For_Alliance_Leader() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithLeader(1);

            var handler = new GetCurrentAllianceRightsQueryHandler(builder.Context);
            var query = new GetCurrentAllianceRightsQuery("test1@test.com");

            var result = handler.Execute(query);

            result.IsInAlliance.Should().BeTrue();
            result.CanInvite.Should().BeTrue();
            result.CanManageRoles.Should().BeTrue();
            result.CanDeleteChatMessages.Should().BeTrue();
            result.CanKickMembers.Should().BeTrue();
            result.CanTransferLeadership.Should().BeTrue();
            result.CanDisbandAlliance.Should().BeTrue();
            result.CanManageNonAggressionPacts.Should().BeTrue();
            result.CanManageWars.Should().BeTrue();
            result.CanBank.Should().BeTrue();
        }
    }
}