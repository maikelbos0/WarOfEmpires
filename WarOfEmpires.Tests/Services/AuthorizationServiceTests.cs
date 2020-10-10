using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries;
using WarOfEmpires.Services;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.Tests.Services {
    [TestClass]
    public sealed class AuthorizationServiceTests {
        [DataTestMethod]
        [DataRow(null, false, false, DisplayName = "Not authenticated")]
        [DataRow("Admin", true, true, DisplayName = "Admin")]
        [DataRow("User", false, false, DisplayName = "User")]
        public void AuthorizationService_IsAdmin_Succeeds(string identity, bool isAdmin, bool expectedIsAdmin) {
            var messageService = Substitute.For<IMessageService>();
            var authorizationService = new AuthorizationService(new FakeAuthenticationService() { Identity = identity }, messageService);

            messageService.Dispatch(Arg.Any<IQuery<bool>>()).Returns(isAdmin);

            authorizationService.IsAdmin().Should().Be(expectedIsAdmin);
        }

        [DataTestMethod]
        [DataRow(null, false, false, DisplayName = "Not authenticated")]
        [DataRow("User", false, false, DisplayName = "Not in alliance")]
        [DataRow("User", true, true, DisplayName = "In alliance")]
        public void AuthorizationService_IsAuthorized_Succeeds_General(string identity, bool isInAlliance, bool expectedOutcome) {
            var messageService = Substitute.For<IMessageService>();
            var request = Substitute.For<IAllianceAuthorizationRequest>();
            var authorizationService = new AuthorizationService(new FakeAuthenticationService() { Identity = identity }, messageService);

            messageService.Dispatch(Arg.Any<IQuery<CurrentAllianceRightsViewModel>>()).Returns(new CurrentAllianceRightsViewModel() {
                IsInAlliance = isInAlliance
            });

            authorizationService.IsAuthorized(request).Should().Be(expectedOutcome);
        }

        [DataTestMethod]
        [DataRow(null, false, false, false, DisplayName = "Not authenticated")]
        [DataRow("User", false, false, false, DisplayName = "Not in alliance")]
        [DataRow("User", true, false, false, DisplayName = "In alliance but can't invite")]
        [DataRow("User", true, true, true, DisplayName = "Can invite")]
        public void AuthorizationService_IsAuthorized_Succeeds_CanInvite(string identity, bool isInAlliance, bool canInvite, bool expectedOutcome) {
            var messageService = Substitute.For<IMessageService>();
            var request = Substitute.For<IAllianceAuthorizationRequest>();
            var authorizationService = new AuthorizationService(new FakeAuthenticationService() { Identity = identity }, messageService);

            messageService.Dispatch(Arg.Any<IQuery<CurrentAllianceRightsViewModel>>()).Returns(new CurrentAllianceRightsViewModel() {
                IsInAlliance = isInAlliance,
                CanInvite = canInvite
            });
            request.CanInvite.Returns(true);

            authorizationService.IsAuthorized(request).Should().Be(expectedOutcome);
        }

        [DataTestMethod]
        [DataRow(null, false, false, false, DisplayName = "Not authenticated")]
        [DataRow("User", false, false, false, DisplayName = "Not in alliance")]
        [DataRow("User", true, false, false, DisplayName = "In alliance but can't manage roles")]
        [DataRow("User", true, true, true, DisplayName = "Can manage roles")]
        public void AuthorizationService_IsAuthorized_Succeeds_CanManageRoles(string identity, bool isInAlliance, bool canManageRoles, bool expectedOutcome) {
            var messageService = Substitute.For<IMessageService>();
            var request = Substitute.For<IAllianceAuthorizationRequest>();
            var authorizationService = new AuthorizationService(new FakeAuthenticationService() { Identity = identity }, messageService);

            messageService.Dispatch(Arg.Any<IQuery<CurrentAllianceRightsViewModel>>()).Returns(new CurrentAllianceRightsViewModel() {
                IsInAlliance = isInAlliance,
                CanManageRoles = canManageRoles
            });
            request.CanManageRoles.Returns(true);

            authorizationService.IsAuthorized(request).Should().Be(expectedOutcome);
        }

        [DataTestMethod]
        [DataRow(null, false, false, false, DisplayName = "Not authenticated")]
        [DataRow("User", false, false, false, DisplayName = "Not in alliance")]
        [DataRow("User", true, false, false, DisplayName = "In alliance but can't delete chat messages")]
        [DataRow("User", true, true, true, DisplayName = "Can delete chat messages")]
        public void AuthorizationService_IsAuthorized_Succeeds_CanDeleteChatMessages(string identity, bool isInAlliance, bool canDeleteChatMessages, bool expectedOutcome) {
            var messageService = Substitute.For<IMessageService>();
            var request = Substitute.For<IAllianceAuthorizationRequest>();
            var authorizationService = new AuthorizationService(new FakeAuthenticationService() { Identity = identity }, messageService);

            messageService.Dispatch(Arg.Any<IQuery<CurrentAllianceRightsViewModel>>()).Returns(new CurrentAllianceRightsViewModel() {
                IsInAlliance = isInAlliance,
                CanDeleteChatMessages = canDeleteChatMessages
            });
            request.CanDeleteChatMessages.Returns(true);

            authorizationService.IsAuthorized(request).Should().Be(expectedOutcome);
        }
    }
}