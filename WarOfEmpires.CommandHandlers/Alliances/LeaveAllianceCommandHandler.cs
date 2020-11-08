using System;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class LeaveAllianceCommandHandler : ICommandHandler<LeaveAllianceCommand> {
        private readonly IPlayerRepository _repository;

        public LeaveAllianceCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<LeaveAllianceCommand> Execute(LeaveAllianceCommand command) {
            var result = new CommandResult<LeaveAllianceCommand>();
            var player = _repository.Get(command.Email);

            if (player.Alliance == null) {
                throw new InvalidOperationException("You must be in an alliance to leave it");
            }

            if (player == player.Alliance.Leader) {
                throw new InvalidOperationException("The alliance leader can't leave the alliance");
            }

            player.Alliance.RemoveMember(player);
            _repository.SaveChanges();

            return result;
        }
    }
}