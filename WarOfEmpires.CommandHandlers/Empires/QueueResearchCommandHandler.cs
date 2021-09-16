﻿using System;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Empires {
    [TransientServiceImplementation(typeof(ICommandHandler<QueueResearchCommand>))]
    public sealed class QueueResearchCommandHandler : ICommandHandler<QueueResearchCommand> {
        private readonly IPlayerRepository _repository;

        public QueueResearchCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<QueueResearchCommand> Execute(QueueResearchCommand command) {
            var result = new CommandResult<QueueResearchCommand>();
            var player = _repository.Get(command.Email);
            var researchType = (ResearchType)Enum.Parse(typeof(ResearchType), command.ResearchType);

            player.QueueResearch(researchType);
            _repository.SaveChanges();

            return result;
        }
    }
}
