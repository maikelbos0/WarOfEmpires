﻿using System.Linq;
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
            var attackId = int.Parse(command.AttackId);
            var attack = player.ReceivedAttacks.Single(m => m.Id == attackId);

            attack.IsRead = true;

            _repository.Update();

            return result;
        }
    }
}