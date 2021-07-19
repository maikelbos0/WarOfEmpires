using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Players;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Players {
    [TransientServiceImplementation(typeof(ICommandHandler<UnblockPlayerCommand>))]
    public sealed class UnblockPlayerCommandHandler : ICommandHandler<UnblockPlayerCommand> {
        private readonly IPlayerRepository _repository;

        public UnblockPlayerCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<UnblockPlayerCommand> Execute(UnblockPlayerCommand command) {
            throw new System.NotImplementedException();
        }
    }
}
