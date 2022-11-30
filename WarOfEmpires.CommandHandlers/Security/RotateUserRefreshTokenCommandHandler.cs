using System;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Repositories.Security;

namespace WarOfEmpires.CommandHandlers.Security {
    public sealed class RotateUserRefreshTokenCommandHandler : ICommandHandler<RotateUserRefreshTokenCommand> {
        private readonly IUserRepository _repository;

        public RotateUserRefreshTokenCommandHandler(IUserRepository repository) {
            _repository = repository;
        }

        public CommandResult<RotateUserRefreshTokenCommand> Execute(RotateUserRefreshTokenCommand command) {
            var result = new CommandResult<RotateUserRefreshTokenCommand>();
            var user = _repository.GetActiveByEmail(command.Email);

            if (user.RotateRefreshToken(command.RequestId, Convert.FromBase64String(command.RefreshToken))) {
                _repository.SaveChanges();
            }
            else {
                result.AddError("Invalid refresh token");
            }

            return result;
        }
    }
}