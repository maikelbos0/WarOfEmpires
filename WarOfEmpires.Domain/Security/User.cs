using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace WarOfEmpires.Domain.Security {
    public class User : AggregateRoot {
        public virtual UserStatus Status { get; protected set; }
        public virtual bool IsAdmin { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual int? ActivationCode { get; protected set; }
        public virtual Password Password { get; protected set; }
        public virtual TemporaryPassword PasswordResetToken { get; protected set; } = TemporaryPassword.None;
        public virtual string NewEmail { get; protected set; }
        public virtual int? NewEmailConfirmationCode { get; protected set; }
        public virtual DateTime? LastOnline { get; protected set; }
        public virtual ICollection<UserEvent> UserEvents { get; protected set; } = new List<UserEvent>();
        public virtual ICollection<RefreshTokenFamily> RefreshTokenFamilies { get; protected set; } = new List<RefreshTokenFamily>();

        protected User() {
        }

        public User(string email, string password) : this() {
            Email = email;
            Password = new Password(password);
            Status = UserStatus.New;
            ActivationCode = GetNewActivationCode();
            AddEvent(UserEventType.Registered);
        }

        protected void AddEvent(UserEventType userEventType) {
            UserEvents.Add(new UserEvent(this, userEventType));
        }

        public virtual byte[] LogIn() {
            var family = new RefreshTokenFamily(GetNewRefreshToken());

            // If we log in, the password reset is not needed anymore and leaving it is a security risk
            PasswordResetToken = TemporaryPassword.None;
            RefreshTokenFamilies.Add(family);
            AddEvent(UserEventType.LoggedIn);

            return family.CurrentToken;
        }

        public virtual bool RotateRefreshToken(byte[] currentToken, out byte[] newToken) {
            var family = RefreshTokenFamilies.SingleOrDefault(f => f.CurrentToken.SequenceEqual(currentToken));

            if (family != null) {
                newToken = GetNewRefreshToken();
                family.RotateRefreshToken(newToken);
                AddEvent(UserEventType.RefreshTokenRotated);
                return true;
            }
            else {
                var reusedFamilies = RefreshTokenFamilies.Where(f => f.ExpiredRefreshTokens.Any(t => t.Token.SequenceEqual(currentToken))).ToList();

                foreach (var reusedFamily in reusedFamilies) {
                    RefreshTokenFamilies.Remove(reusedFamily);
                }

                AddEvent(UserEventType.FailedRefreshTokenValidation);
                newToken = null;
                return false;
            }
        }

        private static byte[] GetNewRefreshToken() {
            return RandomNumberGenerator.GetBytes(100);
        }

        public virtual void LogInFailed() {
            AddEvent(UserEventType.FailedLogIn);
        }

        public virtual void LogOut() {
            AddEvent(UserEventType.LoggedOut);
        }

        public virtual string GeneratePasswordResetToken() {
            var token = GetNewPasswordResetToken();

            PasswordResetToken = new TemporaryPassword(token);
            AddEvent(UserEventType.PasswordResetRequested);

            return token;
        }

        public virtual void PasswordResetRequestFailed() {
            AddEvent(UserEventType.FailedPasswordResetRequest);
        }

        public virtual void Activate() {
            Status = UserStatus.Active;
            ActivationCode = null;
            AddEvent(UserEventType.Activated);
        }

        public virtual void ActivationFailed() {
            AddEvent(UserEventType.FailedActivation);
        }

        public virtual void ChangePassword(string password) {
            Password = new Password(password);
            AddEvent(UserEventType.PasswordChanged);
        }

        public virtual void ChangePasswordFailed() {
            AddEvent(UserEventType.FailedPasswordChange);
        }

        public virtual void ResetPassword(string password) {
            PasswordResetToken = TemporaryPassword.None;
            Password = new Password(password);
            AddEvent(UserEventType.PasswordReset);
        }

        public virtual void ResetPasswordFailed() {
            AddEvent(UserEventType.FailedPasswordReset);
        }

        public virtual void GenerateActivationCode() {
            ActivationCode = GetNewActivationCode();
            AddEvent(UserEventType.ActivationCodeSent);
        }

        public virtual void Deactivate() {
            Status = UserStatus.Inactive;
            AddEvent(UserEventType.Deactivated);
        }

        public virtual void DeactivationFailed() {
            AddEvent(UserEventType.FailedDeactivation);
        }

        public virtual void RequestEmailChange(string email) {
            NewEmail = email;
            NewEmailConfirmationCode = GetNewActivationCode();
            AddEvent(UserEventType.EmailChangeRequested);
        }

        public virtual void RequestEmailChangeFailed() {
            AddEvent(UserEventType.FailedEmailChangeRequest);
        }

        public virtual void ChangeEmail() {
            Email = NewEmail;
            NewEmail = null;
            NewEmailConfirmationCode = null;
            AddEvent(UserEventType.EmailChanged);
        }

        public virtual void ChangeEmailFailed() {
            AddEvent(UserEventType.FailedEmailChange);
        }

        protected static string GetNewPasswordResetToken() {
            var tokenCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
            var tokenBuilder = new StringBuilder();

            for (var i = 0; i < 20; i++) {
                tokenBuilder.Append(tokenCharacters[RandomNumberGenerator.GetInt32(tokenCharacters.Length)]);
            }

            return tokenBuilder.ToString();
        }

        protected static int GetNewActivationCode() {
            return RandomNumberGenerator.GetInt32(9999, int.MaxValue) + 1;
        }

        public virtual void WasOnline() {
            LastOnline = DateTime.UtcNow;
        }

        public void Update(string email, UserStatus status, bool isAdmin) {
            Email = email;
            Status = status;
            IsAdmin = isAdmin;
        }
    }
}