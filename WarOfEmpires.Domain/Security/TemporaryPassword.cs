using System;

namespace WarOfEmpires.Domain.Security {
    public class TemporaryPassword : Password {
        public static TemporaryPassword None => new TemporaryPassword();

        private readonly int _expirationInSeconds = 3600;

        public DateTime? ExpiryDate { get; private set; }

        protected TemporaryPassword() : base() {
        }

        public TemporaryPassword(string password) : base(password) {
            ExpiryDate = DateTime.UtcNow.AddSeconds(_expirationInSeconds);
        }

        public override bool Verify(string password) {
            return ExpiryDate > DateTime.UtcNow
                && base.Verify(password);
        }
    }
}