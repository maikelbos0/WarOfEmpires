﻿using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Empires {
    [TransientServiceImplementation(typeof(ICommandHandler<SetTaxCommand>))]
    public sealed class SetTaxCommandHandler : ICommandHandler<SetTaxCommand> {
        private readonly IPlayerRepository _repository;

        public SetTaxCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<SetTaxCommand> Execute(SetTaxCommand command) {
            var result = new CommandResult<SetTaxCommand>();
            var player = _repository.Get(command.Email);

            player.Tax = command.Tax;
            _repository.SaveChanges();

            return result;
        }
    }
}