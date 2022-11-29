using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.QueryHandlers.Security;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Security {
    [TestClass]
    public sealed class GetUserClaimsQueryHandlerTests {
        [TestMethod]
        public void GetUserClaimsQueryHandler_Returns_Correct_Information() {
            var requestId = Guid.NewGuid();
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithRefreshTokenFamily(1, requestId, new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 });

            builder.User.IsAdmin.Returns(false);

            var handler = new GetUserClaimsQueryHandler(builder.Context);
            var query = new GetUserClaimsQuery("test1@test.com", requestId);

            var result = handler.Execute(query);

            result.Subject.Should().Be("test1@test.com");
            result.RefreshToken.Should().Be(Convert.ToBase64String(new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 }));
            result.IsAdmin.Should().BeFalse();
            result.IsPlayer.Should().BeTrue();
            result.IsInAlliance.Should().BeFalse();
            result.CanTransferLeadership.Should().BeFalse();
            result.CanInvite.Should().BeFalse();
            result.CanManageRoles.Should().BeFalse();
            result.CanLeaveAlliance.Should().BeFalse();
            result.CanDisbandAlliance.Should().BeFalse();
            result.CanManageNonAggressionPacts.Should().BeFalse();
            result.CanManageWars.Should().BeFalse();
            result.CanBank.Should().BeFalse();
            result.DisplayName.Should().Be("Test display name 1");
        }

        [TestMethod]
        public void GetUserClaimsQueryHandler_Returns_Correct_Information_IsPlayer() {
            var builder = new FakeBuilder()
                .BuildUser(1);

            var handler = new GetUserClaimsQueryHandler(builder.Context);
            var query = new GetUserClaimsQuery("test1@test.com", Guid.NewGuid());

            var result = handler.Execute(query);

            result.IsPlayer.Should().BeFalse();
        }

        [TestMethod]
        public void GetUserClaimsQueryHandler_Returns_Correct_Information_IsAdmin() {
            var builder = new FakeBuilder()
                .BuildUser(1);

            builder.User.IsAdmin.Returns(true);

            var handler = new GetUserClaimsQueryHandler(builder.Context);
            var query = new GetUserClaimsQuery("test1@test.com", Guid.NewGuid());

            var result = handler.Execute(query);

            result.IsAdmin.Should().BeTrue();
        }

        [TestMethod]
        public void GetUserClaimsQueryHandler_Returns_Correct_Information_IsInAlliance() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .BuildMember(1);

            builder.Player.AllianceRole.Returns((Role)null);

            var handler = new GetUserClaimsQueryHandler(builder.Context);
            var query = new GetUserClaimsQuery("test1@test.com", Guid.NewGuid());

            var result = handler.Execute(query);

            result.IsInAlliance.Should().BeTrue();
            result.CanTransferLeadership.Should().BeFalse();
            result.CanInvite.Should().BeFalse();
            result.CanManageRoles.Should().BeFalse();
            result.CanLeaveAlliance.Should().BeTrue();
            result.CanDisbandAlliance.Should().BeFalse();
            result.CanManageNonAggressionPacts.Should().BeFalse();
            result.CanManageWars.Should().BeFalse();
            result.CanBank.Should().BeFalse();
        }

        [TestMethod]
        public void GetUserClaimsQueryHandler_Returns_Correct_Information_Rights_In_Role() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1, out var player)
                .WithRole(2, "Test", player);

            var handler = new GetUserClaimsQueryHandler(builder.Context);
            var query = new GetUserClaimsQuery("test1@test.com", Guid.NewGuid());

            var result = handler.Execute(query);

            result.CanInvite.Should().BeTrue();
            result.CanManageRoles.Should().BeTrue();
            result.CanLeaveAlliance.Should().BeTrue();
            result.CanTransferLeadership.Should().BeFalse();
            result.CanDisbandAlliance.Should().BeFalse();
            result.CanManageNonAggressionPacts.Should().BeTrue();
            result.CanManageWars.Should().BeTrue();
            result.CanBank.Should().BeTrue();
        }

        [TestMethod]
        public void GetUserClaimsQueryHandler_Returns_Correct_Information_Rights_For_Alliance_Leader() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .BuildLeader(1);

            var handler = new GetUserClaimsQueryHandler(builder.Context);
            var query = new GetUserClaimsQuery("test1@test.com", Guid.NewGuid());

            var result = handler.Execute(query);

            result.CanInvite.Should().BeTrue();
            result.CanManageRoles.Should().BeTrue();
            result.CanLeaveAlliance.Should().BeFalse();
            result.CanTransferLeadership.Should().BeTrue();
            result.CanDisbandAlliance.Should().BeTrue();
            result.CanManageNonAggressionPacts.Should().BeTrue();
            result.CanManageWars.Should().BeTrue();
            result.CanBank.Should().BeTrue();
        }
    }
}
