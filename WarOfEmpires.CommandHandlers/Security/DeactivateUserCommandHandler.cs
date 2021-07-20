using VDT.Core.DependencyInjection;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Repositories.Security;

namespace WarOfEmpires.CommandHandlers.Security {
    [TransientServiceImplementation(typeof(ICommandHandler<DeactivateUserCommand>))]
    public sealed class DeactivateUserCommandHandler : ICommandHandler<DeactivateUserCommand> {
        private readonly IUserRepository _repository;
        private readonly IPlayerRepository _playerRepository;

        public DeactivateUserCommandHandler(IUserRepository repository, IPlayerRepository  playerRepository) {
            _repository = repository;
            _playerRepository = playerRepository;
        }

        public CommandResult<DeactivateUserCommand> Execute(DeactivateUserCommand parameter) {
            var result = new CommandResult<DeactivateUserCommand>();
            var user = _repository.GetActiveByEmail(parameter.Email);
            var player = _playerRepository.Get(user.Id);

            if (!user.Password.Verify(parameter.Password)) {
                result.AddError(c => c.Password, "Invalid password");                
            }

            if (player.Alliance?.Leader == player) {
                result.AddError("You can't deactivate your account while you are leading your alliance; transfer leadership or dissolve your alliance");
            }
            
            if (result.Success) {
                user.Deactivate();
            }
            else {
                user.DeactivationFailed();
            }

            _repository.SaveChanges();

            return result;
        }
    }
}