﻿using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Utilities.Events;

namespace WarOfEmpires.CommandHandlers.Empires {
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