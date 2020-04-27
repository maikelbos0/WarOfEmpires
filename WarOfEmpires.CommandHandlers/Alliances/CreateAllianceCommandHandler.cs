using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class CreateAllianceCommandHandler : ICommandHandler<CreateAllianceCommand> {
        private readonly IAllianceRepository _repository;
        private readonly IPlayerRepository _playerRepository;

        public CreateAllianceCommandHandler(IAllianceRepository repository, IPlayerRepository playerRepository) {
            _repository = repository;
            _playerRepository = playerRepository;
        }

        public CommandResult<CreateAllianceCommand> Execute(CreateAllianceCommand command) {
            var result = new CommandResult<CreateAllianceCommand>();
            var player = _playerRepository.Get(command.Email);

            if (command.Code.Length > 4) {
                result.AddError(c => c.Code, "Code must be 4 characters or less");
            }

            if (result.Success) {
                _repository.Add( new Alliance(player, command.Code, command.Name));
            }

            return result;
        }
    }
}