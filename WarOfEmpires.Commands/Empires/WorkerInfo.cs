namespace WarOfEmpires.Commands.Empires {
    public sealed class WorkerInfo {
        public string Type { get; }
        public int? Count { get; }

        public WorkerInfo(string type, int? count) {
            Type = type;
            Count = count;
        }
    }
}
