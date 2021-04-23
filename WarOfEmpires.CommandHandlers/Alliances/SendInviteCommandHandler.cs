using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [ScopedServiceImplementation(typeof(ICommandHandler<SendInviteCommand>))]
    public sealed class SendInviteCommandHandler : ICommandHandler<SendInviteCommand> {
        private readonly IPlayerRepository _repository;

        public SendInviteCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<SendInviteCommand> Execute(SendInviteCommand command) {
            var result = new CommandResult<SendInviteCommand>();
            var player = _repository.Get(command.PlayerId);
            var member = _repository.Get(command.Email);
            var alliance = member.Alliance;

            if (alliance.Invites.Any(i => i.Player == player)) {
                result.AddError("This player already has an open invite and can not be invited again");
            }

            if (result.Success) {
                alliance.SendInvite(player, command.Subject, command.Body);
                _repository.SaveChanges();
            }

            return result;
        }
    }
}
