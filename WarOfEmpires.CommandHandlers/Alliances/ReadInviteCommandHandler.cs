using System.Linq;
using VDT.Core.DependencyInjection.Attributes;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [TransientServiceImplementation(typeof(ICommandHandler<ReadInviteCommand>))]
    public sealed class ReadInviteCommandHandler : ICommandHandler<ReadInviteCommand> {
        private readonly IPlayerRepository _repository;

        public ReadInviteCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<ReadInviteCommand> Execute(ReadInviteCommand command) {
            var result = new CommandResult<ReadInviteCommand>();
            var player = _repository.Get(command.Email);
            var invite = player.Invites.Single(m => m.Id == command.InviteId);

            invite.IsRead = true;

            _repository.SaveChanges();

            return result;
        }
    }
}