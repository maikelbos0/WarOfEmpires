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
        private readonly IPlayerRepository _repository;

        public AttackCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<AttackCommand> Execute(AttackCommand command) {
            var result = new CommandResult<AttackCommand>();
            var type = (AttackType)Enum.Parse(typeof(AttackType), command.AttackType);
            var attacker = _repository.Get(command.AttackerEmail);
            var defender = _repository.Get(command.DefenderId);

            if (attacker == defender) {
                throw new InvalidOperationException("You can't attack yourself");
            }

            if (attacker.Alliance != null && attacker.Alliance == defender.Alliance) {
                throw new InvalidOperationException("You can't attack an alliance member");
            }

            if (attacker.AttackTurns < command.Turns) {
                result.AddError(c => c.Turns, "You don't have enough attack turns");
            }

            if (result.Success) {
                attacker.ExecuteAttack(type, defender, command.Turns);
                _repository.SaveChanges();
            }

            return result;
        }
    }
}