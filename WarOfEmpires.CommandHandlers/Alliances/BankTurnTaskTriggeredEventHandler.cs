﻿using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Utilities.Events;

namespace WarOfEmpires.CommandHandlers.Alliances {
    public sealed class BankTurnTaskTriggeredEventHandler : IEventHandler<BankTurnTaskTriggeredEvent> {
        private readonly ICommandHandler<AddBankTurnCommand> _commandHandler;

        public BankTurnTaskTriggeredEventHandler(ICommandHandler<AddBankTurnCommand> commandHandler) {
            _commandHandler = commandHandler;
        }

        public void Handle(BankTurnTaskTriggeredEvent domainEvent) {
            _commandHandler.Execute(new AddBankTurnCommand());
        }
    }
}