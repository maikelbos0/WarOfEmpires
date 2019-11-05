using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Security;
using WarOfEmpires.Utilities.Container;
using System;

namespace WarOfEmpires.CommandHandlers.Security {
    [InterfaceInjectable]
    [Audit]
    public sealed class ChangeUserProfileCommandHandler : ICommandHandler<ChangeUserProfileCommand> {
        private readonly IUserRepository _repository;

        public ChangeUserProfileCommandHandler(IUserRepository repository) {
            _repository = repository;
        }

        public CommandResult<ChangeUserProfileCommand> Execute(ChangeUserProfileCommand command) {
            var result = new CommandResult<ChangeUserProfileCommand>();
            var user = _repository.GetActiveByEmail(command.Email);

            user.DisplayName = command.DisplayName;
            user.Description = command.Description;
            user.ShowEmail = command.ShowEmail;

            _repository.Update();

            return result;
        }
    }
}