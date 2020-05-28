using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class ReadInvitesCommandHandler : ICommandHandler<ReadInvitesCommand> {
        private readonly IPlayerRepository _repository;

        public ReadInvitesCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<ReadInvitesCommand> Execute(ReadInvitesCommand command) {
            throw new System.NotImplementedException();
        }
    }
}