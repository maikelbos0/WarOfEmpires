using System;
using System.Linq;
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
            var result = new CommandResult<DeclarePeaceCommand>();
            var alliance = _repository.Get(command.Email);
            var war = alliance.Wars.Single(w => w.Id == command.WarId);

            if (war.PeaceDeclarations.Contains(alliance)) {
                throw new InvalidOperationException("You can't declare peace because peace has already been declared");
            }

            if (result.Success) {
                war.DeclarePeace(alliance);
                _repository.SaveChanges();
            }

            return result;
        }
    }
}
