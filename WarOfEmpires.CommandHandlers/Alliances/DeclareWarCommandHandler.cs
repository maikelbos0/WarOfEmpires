using System;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [TransientServiceImplementation(typeof(ICommandHandler<DeclareWarCommand>))]
    public sealed class DeclareWarCommandHandler : ICommandHandler<DeclareWarCommand> {
        private readonly IAllianceRepository _repository;

        public DeclareWarCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<DeclareWarCommand> Execute(DeclareWarCommand command) {
            var result = new CommandResult<DeclareWarCommand>();
            var sender = _repository.Get(command.Email);
            var recipient = _repository.Get(command.AllianceId);

            if (sender == recipient) {
                throw new InvalidOperationException("You can't declare war on yourself");
            }

            if (recipient.Wars.Any(r => r.Alliances.Any(a => a == sender))) {
                result.AddError("You are already at war with this alliance");
            }

            if (result.Success) {
                sender.DeclareWar(recipient);
                _repository.SaveChanges();
            }

            return result;
        }
    }
}
