using System;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [TransientServiceImplementation(typeof(ICommandHandler<CancelPeaceDeclarationCommand>))]
    public sealed class CancelPeaceDeclarationCommandHandler : ICommandHandler<CancelPeaceDeclarationCommand> {
        private readonly IAllianceRepository _repository;

        public CancelPeaceDeclarationCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<CancelPeaceDeclarationCommand> Execute(CancelPeaceDeclarationCommand command) {
            throw new NotImplementedException();
        }
    }
}
