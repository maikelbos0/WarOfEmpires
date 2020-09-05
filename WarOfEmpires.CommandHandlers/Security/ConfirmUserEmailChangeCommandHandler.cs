using WarOfEmpires.Commands.Security;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Security;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Security {
    [InterfaceInjectable]
    public sealed class ConfirmUserEmailChangeCommandHandler : ICommandHandler<ConfirmUserEmailChangeCommand> {
        private readonly IUserRepository _repository;

        public ConfirmUserEmailChangeCommandHandler(IUserRepository repository) {
            _repository = repository;
        }

        public CommandResult<ConfirmUserEmailChangeCommand> Execute(ConfirmUserEmailChangeCommand command) {
            var result = new CommandResult<ConfirmUserEmailChangeCommand>();
            var user = _repository.TryGetByEmail(command.Email);

            // Any error that occurs gets the same message to prevent leaking information
            if (user == null
                || !int.TryParse(command.ConfirmationCode, out int confirmationCode)
                || user.Status != UserStatus.Active
                || user.NewEmailConfirmationCode != confirmationCode) {

                user?.ChangeEmailFailed();
                result.AddError(c => c.ConfirmationCode, "This confirmation code is invalid");
            }
            else {
                user.ChangeEmail();
            }

            _repository.SaveChanges();

            return result;
        }
    }
}