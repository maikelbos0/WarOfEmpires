using System;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Attacks;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Game;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Repositories.Game;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Attacks {
    [TransientServiceImplementation(typeof(ICommandHandler<AttackCommand>))]
    public sealed class AttackCommandHandler : ICommandHandler<AttackCommand> {
        private readonly IPlayerRepository _repository;
        private readonly IGameStatusRepository _gameStatusRepository;

        public AttackCommandHandler(IPlayerRepository repository, IGameStatusRepository gameStatusRepository) {
            _repository = repository;
            _gameStatusRepository = gameStatusRepository;
        }

        [Audit]
        public CommandResult<AttackCommand> Execute(AttackCommand command) {
            var result = new CommandResult<AttackCommand>();
            var type = (AttackType)Enum.Parse(typeof(AttackType), command.AttackType);
            var attacker = _repository.Get(command.AttackerEmail);
            var defender = _repository.Get(command.DefenderId);

            if (_gameStatusRepository.Get().Phase == GamePhase.Truce) {
                throw new InvalidOperationException("You can't attack during a truce");
            }

            if (attacker == defender) {
                throw new InvalidOperationException("You can't attack yourself");
            }

            if (attacker.Alliance != null && attacker.Alliance == defender.Alliance) {
                throw new InvalidOperationException("You can't attack an alliance member");
            }

            if (attacker.Alliance != null && attacker.Alliance.NonAggressionPacts.Any(p => p.Alliances.Contains(defender.Alliance))) {
                throw new InvalidOperationException("You can't attack an alliance member from an alliance you're in a non-aggression pact with");
            }

            if (type == AttackType.GrandOverlordAttack && defender.Title != TitleType.GrandOverlord) {
                result.AddError("Your opponent is not the Grand Overlord");
            }

            if (type == AttackType.GrandOverlordAttack && attacker.Title != TitleType.Overlord) {
                result.AddError("You need to be an Overlord to attack the Grand Overlord");
            }

            // TODO filter revenge

            //TODO add hidden fields for defender on execute attack view

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