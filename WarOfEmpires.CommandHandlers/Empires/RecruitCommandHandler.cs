﻿using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Empires {
    public sealed class RecruitCommandHandler : ICommandHandler<RecruitCommand> {
        private readonly IPlayerRepository _repository;

        public RecruitCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<RecruitCommand> Execute(RecruitCommand command) {
            var result = new CommandResult<RecruitCommand>();

            foreach (var player in _repository.GetAll()) {
                player.Recruit();
            }

            _repository.SaveChanges();

            return result;
        }
    }
}