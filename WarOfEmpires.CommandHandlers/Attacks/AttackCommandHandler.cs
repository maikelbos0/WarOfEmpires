using System;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Attacks;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Attacks {
    [InterfaceInjectable]
    [Audit]
    public sealed class AttackCommandHandler : ICommandHandler<AttackCommand> {
        private readonly PlayerRepository _repository;

        public AttackCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<AttackCommand> Execute(AttackCommand command) {
            var result = new CommandResult<AttackCommand>();
            var type = (AttackType)Enum.Parse(typeof(AttackType), command.AttackType);
            var attacker = _repository.Get(command.AttackerEmail);
            var defender = _repository.Get(int.Parse(command.DefenderId));

            if (!int.TryParse(command.Turns, out int turns) || turns < 1 || turns > 10) {
                result.AddError(c => c.Turns, "Turns must be a valid number");
            }

            if (attacker.AttackTurns < turns) {
                result.AddError(c => c.Turns, "You don't have enough attack turns");
            }

            if (result.Success) {
                var attack = AttackFactory.Get(type, attacker, defender, turns);

                attack.Execute();
                attacker.ExecutedAttacks.Add(attack);
                defender.ReceivedAttacks.Add(attack);

                _repository.Update();

                result.ResultId = attack.Id;
            }

            return result;
        }
    }
}