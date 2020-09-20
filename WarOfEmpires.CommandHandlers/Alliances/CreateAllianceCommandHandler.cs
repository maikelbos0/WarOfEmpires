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
        private readonly IPlayerRepository _playerRepository;
        private readonly IAllianceRepository _allianceRepository;

        public CreateAllianceCommandHandler(IPlayerRepository playerRepository, IAllianceRepository allianceRepository) {
            _playerRepository = playerRepository;
            _allianceRepository = allianceRepository;
        }

        public CommandResult<CreateAllianceCommand> Execute(CreateAllianceCommand command) {
            var result = new CommandResult<CreateAllianceCommand>();
            var player = _playerRepository.Get(command.Email);

            if (command.Code.Length > 4) {
                result.AddError(c => c.Code, "Code must be 4 characters or less");
            }

            if (player.Alliance != null) {
                result.AddError("You are already in an alliance; you have to leave before you can create an alliance");
            }

            if (result.Success) {
                _allianceRepository.Add(new Alliance(player, command.Code, command.Name));
            }

            return result;
        }
    }
}