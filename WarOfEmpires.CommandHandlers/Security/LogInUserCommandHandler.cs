using WarOfEmpires.Commands.Security;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Security;

namespace WarOfEmpires.CommandHandlers.Security {
    public sealed class LogInUserCommandHandler : ICommandHandler<LogInUserCommand> {
        private readonly IUserRepository _repository;

        public LogInUserCommandHandler(IUserRepository repository) {
            _repository = repository;
        }

        public CommandResult<LogInUserCommand> Execute(LogInUserCommand command) {
            var result = new CommandResult<LogInUserCommand>();
            var user = _repository.TryGetByEmail(command.Email);

            if (user == null || user.Status != UserStatus.Active || !user.Password.Verify(command.Password)) {
                result.AddError("Invalid email or password");
                user?.LogInFailed();
            }
            else {
                user.LogIn();
            }

            _repository.SaveChanges();

            return result;
        }
    }
}