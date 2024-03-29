﻿using System.Linq;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Alliances {
    public sealed class AcceptInviteCommandHandler : ICommandHandler<AcceptInviteCommand> {
        private readonly IPlayerRepository _repository;

        public AcceptInviteCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<AcceptInviteCommand> Execute(AcceptInviteCommand command) {
            var result = new CommandResult<AcceptInviteCommand>();
            var player = _repository.Get(command.Email);
            var invite = player.Invites.Single(i => i.Id == command.InviteId);

            if (player.Alliance != null) {
                result.AddError("You are already in an alliance; leave your current alliance before accepting an invite");
            }

            if (result.Success) {
                invite.Alliance.AcceptInvite(invite);
                _repository.SaveChanges();
            }

            return result;
        }
    }
}