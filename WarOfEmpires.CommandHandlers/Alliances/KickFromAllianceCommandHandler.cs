using System;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [TransientServiceImplementation(typeof(ICommandHandler<KickFromAllianceCommand>))]
    public sealed class KickFromAllianceCommandHandler : ICommandHandler<KickFromAllianceCommand> {
        private readonly IAllianceRepository _repository;

        public KickFromAllianceCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<KickFromAllianceCommand> Execute(KickFromAllianceCommand command) {
            var result = new CommandResult<KickFromAllianceCommand>();
            var alliance = _repository.Get(command.Email);
            var member = alliance.Members.Single(m => m.Id == int.Parse(command.PlayerID));

            if (member == alliance.Leader) {
                throw new InvalidOperationException("The alliance leader can't leave the alliance");
            }

            alliance.Kick(member);
            _repository.SaveChanges();

            return result;
        }
    }
}