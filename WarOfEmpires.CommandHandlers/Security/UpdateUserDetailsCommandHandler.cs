using System;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Repositories.Security;

namespace WarOfEmpires.CommandHandlers.Security {
    [TransientServiceImplementation(typeof(ICommandHandler<UpdateUserDetailsCommand>))]
    public sealed class UpdateUserDetailsCommandHandler : ICommandHandler<UpdateUserDetailsCommand> {
        private readonly IUserRepository repository;

        public UpdateUserDetailsCommandHandler(IUserRepository repository) {
            this.repository = repository;
        }

        [Audit]
        public CommandResult<UpdateUserDetailsCommand> Execute(UpdateUserDetailsCommand command) {
            throw new NotImplementedException();
        }
    }
}
