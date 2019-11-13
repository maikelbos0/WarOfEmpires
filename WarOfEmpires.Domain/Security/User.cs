using System;
using System.Collections.Generic;
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
        public virtual string NewEmail { get; set; }
        public virtual int? NewEmailConfirmationCode { get; set; }
        public ICollection<UserEvent> UserEvents { get; protected set; } = new List<UserEvent>();

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

        public virtual void LogIn() {
            // If we log in, the password reset is not needed anymore and leaving it is a security risk
            PasswordResetToken = TemporaryPassword.None;
            AddEvent(UserEventType.LoggedIn);
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

        protected string GetNewPasswordResetToken() {
            using (var rng = new RNGCryptoServiceProvider()) {
                // Establish a maximum based on the amount of characters to prevent bias
                var tokenCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
                var maximumNumber = (byte.MaxValue / tokenCharacters.Length) * tokenCharacters.Length;
                var tokenBuilder = new StringBuilder();
                byte[] buffer = new byte[1];

                for (var i = 0; i < 20; i++) {
                    // Get a new number as long as we're at or over the maximum number
                    do {
                        rng.GetBytes(buffer);
                    }
                    while (buffer[0] >= maximumNumber);

                    tokenBuilder.Append(tokenCharacters[buffer[0] % tokenCharacters.Length]);
                }

                return tokenBuilder.ToString();
            }
        }

        protected int GetNewActivationCode() {
            using (var rng = new RNGCryptoServiceProvider()) {
                byte[] buffer = new byte[4];
                int activationCode = 0;

                while (activationCode < 10000) {
                    rng.GetBytes(buffer);

                    activationCode = BitConverter.ToInt32(buffer, 0);
                }

                return activationCode;
            }
        }
    }
}