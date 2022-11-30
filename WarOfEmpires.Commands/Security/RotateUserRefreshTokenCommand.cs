using System;

namespace WarOfEmpires.Commands.Security {
    public sealed class RotateUserRefreshTokenCommand : ICommand {
        public string Email { get; }
        public Guid RequestId { get; }
        public string RefreshToken { get; }

        public RotateUserRefreshTokenCommand(string email, Guid requestId, string refreshToken) {
            Email = email;
            RequestId = requestId;
            RefreshToken = refreshToken;
        }
    }
}
