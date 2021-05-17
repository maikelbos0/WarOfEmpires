using VDT.Core.DependencyInjection;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Repositories.Security;

namespace WarOfEmpires.CommandHandlers.Security {
    [TransientServiceImplementation(typeof(ICommandHandler<DeactivateUserCommand>))]
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
                result.AddError(c => c.Password, "Invalid password");
                user.DeactivationFailed();
            }

            _repository.SaveChanges();

            return result;
        }
    }
}