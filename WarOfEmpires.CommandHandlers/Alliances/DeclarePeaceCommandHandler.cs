using System;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [TransientServiceImplementation(typeof(ICommandHandler<DeclarePeaceCommand>))]
    public sealed class DeclarePeaceCommandHandler : ICommandHandler<DeclarePeaceCommand> {
        private readonly IAllianceRepository _repository;

        public DeclarePeaceCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<DeclarePeaceCommand> Execute(DeclarePeaceCommand command) {
            throw new NotImplementedException();
        }
    }
}
