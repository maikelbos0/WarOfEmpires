using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Attacks;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Attacks {
    [InterfaceInjectable]
    [Audit]
    public sealed class ReadAttackCommandHandler : ICommandHandler<ReadAttackCommand> {
        private readonly PlayerRepository _repository;

        public ReadAttackCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<ReadAttackCommand> Execute(ReadAttackCommand command) {
            var result = new CommandResult<ReadAttackCommand>();
            var player = _repository.Get(command.Email);
            var AttackId = int.Parse(command.AttackId);
            var Attack = player.ReceivedAttacks.Single(m => m.Id == AttackId);

            Attack.IsRead = true;

            _repository.Update();

            return result;
        }
    }
}