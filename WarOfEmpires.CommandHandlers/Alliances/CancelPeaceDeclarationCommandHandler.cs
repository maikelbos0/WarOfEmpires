using System;
using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;

namespace WarOfEmpires.CommandHandlers.Alliances {
    public sealed class CancelPeaceDeclarationCommandHandler : ICommandHandler<CancelPeaceDeclarationCommand> {
        private readonly IAllianceRepository _repository;

        public CancelPeaceDeclarationCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<CancelPeaceDeclarationCommand> Execute(CancelPeaceDeclarationCommand command) {
            var result = new CommandResult<CancelPeaceDeclarationCommand>();
            var alliance = _repository.Get(command.Email);
            var war = alliance.Wars.Single(w => w.Id == command.WarId);

            if (!war.PeaceDeclarations.Contains(alliance)) {
                throw new InvalidOperationException("You can't cancel a peace declaration because peace has not been declared");
            }

            if (result.Success) {
                war.CancelPeaceDeclaration(alliance);
                _repository.SaveChanges();
            }

            return result;
        }
    }
}
