using WarOfEmpires.Commands.Security;
using WarOfEmpires.Repositories.Security;

namespace WarOfEmpires.CommandHandlers.Security {
    public sealed class GenerateUserRefreshTokenCommandHandler : ICommandHandler<GenerateUserRefreshTokenCommand> {
        private readonly IUserRepository _repository;

        public GenerateUserRefreshTokenCommandHandler(IUserRepository repository) {
            _repository = repository;
        }

        public CommandResult<GenerateUserRefreshTokenCommand> Execute(GenerateUserRefreshTokenCommand command) {
            var result = new CommandResult<GenerateUserRefreshTokenCommand>();
            var user = _repository.GetActiveByEmail(command.Email);

            user.GenerateRefreshToken(command.RequestId);
            _repository.SaveChanges();

            return result;
        }
    }
}