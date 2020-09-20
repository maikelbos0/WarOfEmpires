using WarOfEmpires.Commands.Security;
using WarOfEmpires.Repositories.Security;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Security {
    [InterfaceInjectable]
    public sealed class ChangeUserPasswordCommandHandler : ICommandHandler<ChangeUserPasswordCommand> {
        private readonly IUserRepository _repository;

        public ChangeUserPasswordCommandHandler(IUserRepository repository) {
            _repository = repository;
        }

        public CommandResult<ChangeUserPasswordCommand> Execute(ChangeUserPasswordCommand parameter) {
            var result = new CommandResult<ChangeUserPasswordCommand>();
            var user = _repository.GetActiveByEmail(parameter.Email);

            if (user.Password.Verify(parameter.CurrentPassword)) {
                user.ChangePassword(parameter.NewPassword);
            }
            else { 
                result.AddError(p => p.CurrentPassword, "Invalid password");
                user.ChangePasswordFailed();
            }

            _repository.SaveChanges();

            return result;
        }
    }
}