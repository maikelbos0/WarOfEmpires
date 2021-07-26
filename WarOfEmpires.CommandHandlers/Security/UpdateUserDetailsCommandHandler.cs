using System;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Repositories.Security;

namespace WarOfEmpires.CommandHandlers.Security {
    [TransientServiceImplementation(typeof(ICommandHandler<UpdateUserDetailsCommand>))]
    public sealed class UpdateUserDetailsCommandHandler : ICommandHandler<UpdateUserDetailsCommand> {
        private readonly IUserRepository _repository;
        private readonly IPlayerRepository _playerRepository;

        public UpdateUserDetailsCommandHandler(IUserRepository repository, IPlayerRepository playerRepository) {
            _repository = repository;
            _playerRepository = playerRepository;
        }

        [Audit]
        public CommandResult<UpdateUserDetailsCommand> Execute(UpdateUserDetailsCommand command) {
            throw new NotImplementedException();
        }
    }
}
