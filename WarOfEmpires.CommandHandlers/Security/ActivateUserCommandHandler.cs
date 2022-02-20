using WarOfEmpires.Commands.Security;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Security;

namespace WarOfEmpires.CommandHandlers.Security {
    public sealed class ActivateUserCommandHandler : ICommandHandler<ActivateUserCommand> {
        private readonly IUserRepository _repository;

        public ActivateUserCommandHandler(IUserRepository repository) {
            _repository = repository;
        }

        public CommandResult<ActivateUserCommand> Execute(ActivateUserCommand parameter) {
            var result = new CommandResult<ActivateUserCommand>();
            var user = _repository.TryGetByEmail(parameter.Email);

            // Any error that occurs gets the same message to prevent leaking information
            if (user == null
                || !int.TryParse(parameter.ActivationCode, out int activationCode)
                || user.Status != UserStatus.New
                || user.ActivationCode != activationCode) {

                user?.ActivationFailed();
                result.AddError(c => c.ActivationCode, "This activation code is invalid");
            }
            else {
                user.Activate();
            }

            _repository.SaveChanges();

            return result;
        }
    }
}
