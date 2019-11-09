using WarOfEmpires.Commands.Players;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Repositories.Security;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Players {
    [InterfaceInjectable]
    public sealed class RegisterPlayerCommandHandler : ICommandHandler<RegisterPlayerCommand> {
        private readonly IUserRepository _userRepository;
        private readonly IPlayerRepository _repository;

        public RegisterPlayerCommandHandler(IUserRepository userRepository, IPlayerRepository repository) {
            _userRepository = userRepository;
            _repository = repository;
        }

        public CommandResult<RegisterPlayerCommand> Execute(RegisterPlayerCommand command) {
            var result = new CommandResult<RegisterPlayerCommand>();
            var user = _userRepository.TryGetByEmail(command.Email);

            _repository.Add(new Player(user.Id, command.DisplayName));

            return result;
        }
    }
}