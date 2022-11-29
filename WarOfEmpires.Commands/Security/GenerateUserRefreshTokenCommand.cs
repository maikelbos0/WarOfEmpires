using System;

namespace WarOfEmpires.Commands.Security {
    public sealed class GenerateUserRefreshTokenCommand : ICommand {
        public string Email { get; }
        public Guid RequestId { get; }

        public GenerateUserRefreshTokenCommand(string email, Guid requestId) {
            Email = email;
            RequestId = requestId;
        }
    }
}
