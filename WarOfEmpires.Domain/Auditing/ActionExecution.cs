using System;

namespace WarOfEmpires.Domain.Auditing {
    public class ActionExecution : AggregateRoot {
        public DateTime Date { get; protected set; }
        public string Type { get; protected set; }
        public string Data { get; protected set; }
        public double ElapsedMilliseconds { get; protected set; }

        public ActionExecution(string type, string data, double elapsedMilliseconds) {
            Date = DateTime.UtcNow;
            Type = type;
            Data = data;
            ElapsedMilliseconds = elapsedMilliseconds;
        }
    }
}
