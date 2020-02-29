using WarOfEmpires.Domain.Empires;

namespace WarOfEmpires.CommandHandlers.Empires {
    internal class WorkerInfo {
        public WorkerType Type { get; }
        public int Count { get; }

        public WorkerInfo(WorkerType type, int count) {
            Type = type;
            Count = count;
        }
    }
}