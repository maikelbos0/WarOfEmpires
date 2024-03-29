using WarOfEmpires.Domain.Security;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace WarOfEmpires.Domain.Tests.Security {
    [TestClass]
    public sealed class UserTests {
        [TestMethod]
        public void User_Register_Succeeds() {
            var user = new User("test@test.com", "test");

            user.ActivationCode.Should().NotBeNull();
            user.Status.Should().Be(UserStatus.New);
            user.UserEvents.Last().Type.Should().Be(UserEventType.Registered);
            user.UserEvents.Last().Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void User_LogIn_Succeeds() {
            var user = new User("test@test.com", "test");

            user.Activate();
            user.GeneratePasswordResetToken();
            
            user.LogIn();

            user.PasswordResetToken.Should().Be(TemporaryPassword.None);
            user.UserEvents.Last().Type.Should().Be(UserEventType.LoggedIn);
            user.UserEvents.Last().Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void User_GenerateRefreshToken_Succeeds() {
            var user = new User("test@test.com", "test");
            var requestId = Guid.NewGuid();

            user.Activate();            

            user.GenerateRefreshToken(requestId);

            user.UserEvents.Last().Type.Should().Be(UserEventType.RefreshTokenGenerated);
            user.UserEvents.Last().Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            user.RefreshTokenFamilies.Should().HaveCount(1);
            user.RefreshTokenFamilies.Single().CurrentToken.Should().HaveCount(100);
            user.RefreshTokenFamilies.Single().RequestId.Should().Be(requestId);
            user.RefreshTokenFamilies.Single().PreviousRefreshTokens.Should().BeEmpty();
        }

        [TestMethod]
        public void User_RotateRefreshToken_Succeeds() {
            var user = new User("test@test.com", "test");
            var requestId = Guid.NewGuid();
            var family = new RefreshTokenFamily(Guid.NewGuid(), new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 });
            
            user.RefreshTokenFamilies.Add(family);

            var result = user.RotateRefreshToken(requestId, new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 });

            result.Should().BeTrue();
            family.CurrentToken.Should().NotBeEquivalentTo(new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 });
            family.RequestId.Should().Be(requestId);
            family.PreviousRefreshTokens.Should().HaveCount(1);
            family.PreviousRefreshTokens.Single().Token.Should().BeEquivalentTo(new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 });
            user.RefreshTokenFamilies.Should().Contain(family);
            user.UserEvents.Last().Type.Should().Be(UserEventType.RefreshTokenRotated);
            user.UserEvents.Last().Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void User_RotateRefreshToken_Returns_False_For_Invalid_Token() {
            var user = new User("test@test.com", "test");
            var requestId = Guid.NewGuid();
            var family = new RefreshTokenFamily(requestId, new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 });

            user.RefreshTokenFamilies.Add(family);

            var result = user.RotateRefreshToken(Guid.NewGuid(), new byte[] { 2, 2, 2, 2, 2, 2, 2, 2 });

            result.Should().BeFalse();
            family.PreviousRefreshTokens.Should().BeEmpty();
            family.CurrentToken.Should().BeEquivalentTo(new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 });
            family.RequestId.Should().Be(requestId);
            user.RefreshTokenFamilies.Should().Contain(family);
            user.UserEvents.Last().Type.Should().Be(UserEventType.FailedRefreshTokenRotation);
            user.UserEvents.Last().Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void User_RotateRefreshToken_Returns_False_And_Removes_Family_For_Previous_Token() {
            var user = new User("test@test.com", "test");
            var family = new RefreshTokenFamily(Guid.NewGuid(), new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 });

            family.PreviousRefreshTokens.Add(new PreviousRefreshToken(new byte[] { 2, 2, 2, 2, 2, 2, 2, 2 }));
            user.RefreshTokenFamilies.Add(family);

            var result = user.RotateRefreshToken(Guid.NewGuid(), new byte[] { 2, 2, 2, 2, 2, 2, 2, 2 });

            result.Should().BeFalse();
            user.RefreshTokenFamilies.Should().NotContain(family);
            user.UserEvents.Last().Type.Should().Be(UserEventType.FailedRefreshTokenRotation);
            user.UserEvents.Last().Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void User_LogInFailed_Succeeds() {
            var user = new User("test@test.com", "test");

            user.Activate();
            user.LogInFailed();

            user.UserEvents.Last().Type.Should().Be(UserEventType.FailedLogIn);
            user.UserEvents.Last().Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void User_LogOut_Succeeds() {
            var user = new User("test@test.com", "test");

            user.Activate();
            user.LogOut();

            user.UserEvents.Last().Type.Should().Be(UserEventType.LoggedOut);
            user.UserEvents.Last().Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void User_GeneratePasswordResetToken_Succeeds() {
            var user = new User("test@test.com", "test");

            user.Activate();

            var token = user.GeneratePasswordResetToken();

            user.PasswordResetToken.Verify(token).Should().BeTrue();
            user.UserEvents.Last().Type.Should().Be(UserEventType.PasswordResetRequested);
            user.UserEvents.Last().Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void User_Activate_Succeeds() {
            var user = new User("test@test.com", "test");

            user.Activate();

            user.ActivationCode.Should().BeNull();
            user.Status.Should().Be(UserStatus.Active);
            user.UserEvents.Last().Type.Should().Be(UserEventType.Activated);
            user.UserEvents.Last().Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void User_ActivationFailed_Succeeds() {
            var user = new User("test@test.com", "test");

            user.ActivationFailed();

            user.ActivationCode.Should().NotBeNull();
            user.Status.Should().Be(UserStatus.New);
            user.UserEvents.Last().Type.Should().Be(UserEventType.FailedActivation);
            user.UserEvents.Last().Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void User_ChangePassword_Succeeds() {
            var user = new User("test@test.com", "test");

            user.Activate();
            user.ChangePassword("new");

            user.Password.Verify("new").Should().BeTrue();
            user.UserEvents.Last().Type.Should().Be(UserEventType.PasswordChanged);
            user.UserEvents.Last().Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void User_ChangePasswordFailed_Succeeds() {
            var user = new User("test@test.com", "test");

            user.Activate();
            user.ChangePasswordFailed();

            user.UserEvents.Last().Type.Should().Be(UserEventType.FailedPasswordChange);
            user.UserEvents.Last().Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void User_ResetPassword_Succeeds() {
            var user = new User("test@test.com", "test");

            user.Activate();
            user.GeneratePasswordResetToken();
            user.ResetPassword("new");

            user.PasswordResetToken.Should().Be(TemporaryPassword.None);
            user.Password.Verify("new").Should().BeTrue();
            user.UserEvents.Last().Type.Should().Be(UserEventType.PasswordReset);
            user.UserEvents.Last().Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }
        
        [TestMethod]
        public void User_PasswordResetRequestFailed_Succeeds() {
            var user = new User("test@test.com", "test");

            user.Activate();
            user.PasswordResetRequestFailed();

            user.UserEvents.Last().Type.Should().Be(UserEventType.FailedPasswordResetRequest); 
            user.UserEvents.Last().Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void User_ResetPasswordFailed_Succeeds() {
            var user = new User("test@test.com", "test");

            user.Activate();
            user.ResetPasswordFailed();

            user.UserEvents.Last().Type.Should().Be(UserEventType.FailedPasswordReset);
            user.UserEvents.Last().Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void User_GenerateActivationCode_Succeeds() {
            var user = new User("test@test.com", "test");
            var oldActivationCode = user.ActivationCode;

            user.GenerateActivationCode();

            user.ActivationCode.Should().NotBe(oldActivationCode); // This test will fail once every 256 ^ 3 runs
            user.UserEvents.Last().Type.Should().Be(UserEventType.ActivationCodeSent);
            user.UserEvents.Last().Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void User_Deactivate_Succeeds() {
            var user = new User("test@test.com", "test");

            user.Activate();
            user.Deactivate();

            user.Status.Should().Be(UserStatus.Inactive);
            user.UserEvents.Last().Type.Should().Be(UserEventType.Deactivated);
            user.UserEvents.Last().Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void User_DeactivationFailed_Succeeds() {
            var user = new User("test@test.com", "test");

            user.Activate();
            user.DeactivationFailed();

            user.UserEvents.Last().Type.Should().Be(UserEventType.FailedDeactivation);
            user.UserEvents.Last().Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void User_RequestEmailChange_Succeeds() {
            var user = new User("test@test.com", "test");

            user.Activate();
            user.RequestEmailChange("new@test.com");

            user.NewEmailConfirmationCode.Should().NotBeNull();
            user.NewEmail.Should().Be("new@test.com");
            user.UserEvents.Last().Type.Should().Be(UserEventType.EmailChangeRequested);
            user.UserEvents.Last().Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void User_RequestEmailChangeFailed_Succeeds() {
            var user = new User("test@test.com", "test");

            user.Activate();
            user.RequestEmailChangeFailed();

            user.UserEvents.Last().Type.Should().Be(UserEventType.FailedEmailChangeRequest);
            user.UserEvents.Last().Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void User_ChangeEmail_Succeeds() {
            var user = new User("test@test.com", "test");

            user.Activate();
            user.RequestEmailChange("new@test.com");
            user.ChangeEmail();

            user.Email.Should().Be("new@test.com");
            user.NewEmailConfirmationCode.Should().BeNull();
            user.NewEmail.Should().BeNull();
            user.UserEvents.Last().Type.Should().Be(UserEventType.EmailChanged);
            user.UserEvents.Last().Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void User_ChangeEmailFailed_Succeeds() {
            var user = new User("test@test.com", "test");

            user.ChangeEmailFailed();

            user.UserEvents.Last().Type.Should().Be(UserEventType.FailedEmailChange);
            user.UserEvents.Last().Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void User_WasOnline_Succeeds() {
            var user = new User("test@test.com", "test");

            user.WasOnline();

            user.LastOnline.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void User_Update_Succeeds() {
            var user = new User("test@test.com", "test");

            user.Update("new@test.com", UserStatus.Active, true);

            user.Email.Should().Be("new@test.com");
            user.Status.Should().Be(UserStatus.Active);
            user.IsAdmin.Should().BeTrue();
        }
    }
}