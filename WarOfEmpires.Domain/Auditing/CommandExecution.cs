using System;

namespace WarOfEmpires.Domain.Auditing {
    public class CommandExecution : AggregateRoot {
        public DateTime Date { get; protected set; }
        public string CommandType { get; protected set; }
        public string CommandData { get; protected set; }

        public CommandExecution(string commandType, string commandData) {
            Date = DateTime.UtcNow;
            CommandType = commandType;
            CommandData = commandData;
        }
    }
}