using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class LeaveAllianceCommandHandler : ICommandHandler<LeaveAllianceCommand> {
        private readonly IPlayerRepository _repository;

        public LeaveAllianceCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<LeaveAllianceCommand> Execute(LeaveAllianceCommand command) {
            throw new System.NotImplementedException();
        }
    }
}