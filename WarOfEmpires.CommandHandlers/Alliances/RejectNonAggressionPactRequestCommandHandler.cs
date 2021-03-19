﻿using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class RejectNonAggressionPactRequestCommandHandler : ICommandHandler<RejectNonAggressionPactRequestCommand> {
        private readonly IAllianceRepository _repository;

        public RejectNonAggressionPactRequestCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        public CommandResult<RejectNonAggressionPactRequestCommand> Execute(RejectNonAggressionPactRequestCommand command) {
            var result = new CommandResult<RejectNonAggressionPactRequestCommand>();
            var alliance = _repository.Get(command.Email);
            var request = alliance.ReceivedNonAggressionPactRequests.Single(r => r.Id == command.NonAggressionPactRequestId);

            request.Reject();
            _repository.SaveChanges();

            return result;
        }
    }
}