﻿using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class AcceptNonAggressionPactRequestCommandHandler : ICommandHandler<AcceptNonAggressionPactRequestCommand> {
        private readonly IAllianceRepository _repository;

        public AcceptNonAggressionPactRequestCommandHandler(IAllianceRepository repository) {
            _repository = repository;
        }

        public CommandResult<AcceptNonAggressionPactRequestCommand> Execute(AcceptNonAggressionPactRequestCommand command) {
            var result = new CommandResult<AcceptNonAggressionPactRequestCommand>();
            var alliance = _repository.Get(command.Email);
            var request = alliance.ReceivedNonAggressionPactRequests.Single(r => r.Id == command.NonAggressionPactRequestId);

            request.Accept();
            _repository.SaveChanges();

            return result;
        }
    }
}