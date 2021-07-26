using System;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Domain.Security;
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
            var result = new CommandResult<UpdateUserDetailsCommand>();
            var status = (UserStatus)Enum.Parse(typeof(UserStatus), command.Status);
            var player = _playerRepository.Get(command.Id);             // TODO make sure this works for inactive/new players
            var existingUser = _repository.TryGetByEmail(command.Email);

            if (existingUser != null) {
                result.AddError(c => c.Email, "Email address already exists");
            }

            if (result.Success) {
                player.User.Update(command.Email, status, command.IsAdmin);
                player.Update(command.DisplayName);

                if (player.Alliance != null) {
                    player.Alliance.Update(command.AllianceCode, command.AllianceName);
                }

                _repository.SaveChanges();
            }

            return result;
        }
    }
}
