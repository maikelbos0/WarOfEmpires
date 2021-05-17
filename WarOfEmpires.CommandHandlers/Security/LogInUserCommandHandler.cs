using VDT.Core.DependencyInjection;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Security;

namespace WarOfEmpires.CommandHandlers.Security {
    [TransientServiceImplementation(typeof(ICommandHandler<LogInUserCommand>))]
    public sealed class LogInUserCommandHandler : ICommandHandler<LogInUserCommand> {
        private readonly IUserRepository _repository;

        public LogInUserCommandHandler(IUserRepository repository) {
            _repository = repository;
        }

        public CommandResult<LogInUserCommand> Execute(LogInUserCommand parameter) {
            var result = new CommandResult<LogInUserCommand>();
            var user = _repository.TryGetByEmail(parameter.Email);

            if (user == null || user.Status != UserStatus.Active || !user.Password.Verify(parameter.Password)) {
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