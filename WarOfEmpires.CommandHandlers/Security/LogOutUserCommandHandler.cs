using VDT.Core.DependencyInjection.Attributes;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Repositories.Security;

namespace WarOfEmpires.CommandHandlers.Security {
    [TransientServiceImplementation(typeof(ICommandHandler<LogOutUserCommand>))]
    public sealed class LogOutUserCommandHandler : ICommandHandler<LogOutUserCommand> {
        private readonly IUserRepository _repository;

        public LogOutUserCommandHandler(IUserRepository repository) {
            _repository = repository;
        }

        public CommandResult<LogOutUserCommand> Execute(LogOutUserCommand parameter) {
            var result = new CommandResult<LogOutUserCommand>();
            var user = _repository.GetActiveByEmail(parameter.Email);

            user.LogOut();
            _repository.SaveChanges();

            return result;
        }
    }
}