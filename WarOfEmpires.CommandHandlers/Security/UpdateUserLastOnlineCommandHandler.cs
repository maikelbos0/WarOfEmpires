using VDT.Core.DependencyInjection.Attributes;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Repositories.Security;

namespace WarOfEmpires.CommandHandlers.Security {
    [TransientServiceImplementation(typeof(ICommandHandler<UpdateUserLastOnlineCommand>))]
    public sealed class UpdateUserLastOnlineCommandHandler : ICommandHandler<UpdateUserLastOnlineCommand> {
        private readonly IUserRepository _repository;

        public UpdateUserLastOnlineCommandHandler(IUserRepository repository) {
            _repository = repository;
        }

        public CommandResult<UpdateUserLastOnlineCommand> Execute(UpdateUserLastOnlineCommand command) {
            var result = new CommandResult<UpdateUserLastOnlineCommand>();
            var user = _repository.TryGetByEmail(command.Email);

            user.WasOnline();
            _repository.SaveChanges();

            return result;
        }
    }
}