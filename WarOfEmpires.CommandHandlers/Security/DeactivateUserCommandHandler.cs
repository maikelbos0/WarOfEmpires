using WarOfEmpires.Commands.Security;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Security;
using WarOfEmpires.Utilities.Container;
using System;

namespace WarOfEmpires.CommandHandlers.Security {
    [InterfaceInjectable]
    public sealed class DeactivateUserCommandHandler : ICommandHandler<DeactivateUserCommand> {
        private readonly IUserRepository _repository;

        public DeactivateUserCommandHandler(IUserRepository repository) {
            _repository = repository;
        }

        public CommandResult<DeactivateUserCommand> Execute(DeactivateUserCommand parameter) {
            var result = new CommandResult<DeactivateUserCommand>();
            var user = _repository.GetActiveByEmail(parameter.Email);

            if (user.Password.Verify(parameter.Password)) {
                user.Deactivate();
            }
            else {
                result.AddError(p => p.Password, "Invalid password");
                user.DeactivationFailed();
            }

            _repository.SaveChanges();

            return result;
        }
    }
}