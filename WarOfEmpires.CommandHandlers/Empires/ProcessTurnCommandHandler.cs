﻿using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Empires {
    [TransientServiceImplementation(typeof(ICommandHandler<ProcessTurnCommand>))]
    public sealed class ProcessTurnCommandHandler : ICommandHandler<ProcessTurnCommand> {
        private readonly IPlayerRepository _repository;

        public ProcessTurnCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<ProcessTurnCommand> Execute(ProcessTurnCommand command) {
            var result = new CommandResult<ProcessTurnCommand>();

            foreach (var player in _repository.GetAll()) {
                player.ProcessTurn();
            }

            _repository.SaveChanges();

            return result;
        }
    }
}