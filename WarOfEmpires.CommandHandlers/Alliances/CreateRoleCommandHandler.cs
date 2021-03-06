﻿using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [TransientServiceImplementation(typeof(ICommandHandler<CreateRoleCommand>))]
    public sealed class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand> {
        private readonly IAllianceRepository _repository;

        public CreateRoleCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<CreateRoleCommand> Execute(CreateRoleCommand command) {
            var result = new CommandResult<CreateRoleCommand>();
            var alliance = _repository.Get(command.Email);

            alliance.CreateRole(command.Name, command.CanInvite, command.CanManageRoles, command.CanDeleteChatMessages, command.CanKickMembers, command.CanManageNonAggressionPacts, command.CanManageWars);
            _repository.SaveChanges();

            return result;
        }
    }
}