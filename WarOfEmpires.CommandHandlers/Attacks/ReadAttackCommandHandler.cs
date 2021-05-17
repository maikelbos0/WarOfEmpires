using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Attacks;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Attacks {
    [TransientServiceImplementation(typeof(ICommandHandler<ReadAttackCommand>))]
    public sealed class ReadAttackCommandHandler : ICommandHandler<ReadAttackCommand> {
        private readonly IPlayerRepository _repository;

        public ReadAttackCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<ReadAttackCommand> Execute(ReadAttackCommand command) {
            var result = new CommandResult<ReadAttackCommand>();
            var player = _repository.Get(command.Email);
            var attack = player.ReceivedAttacks.Single(m => m.Id == command.AttackId);

            attack.IsRead = true;

            _repository.SaveChanges();

            return result;
        }
    }
}