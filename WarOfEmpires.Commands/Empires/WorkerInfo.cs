namespace WarOfEmpires.Commands.Empires {
    public sealed class WorkerInfo {
        public string Type { get; }
        public string Count { get; }

        public WorkerInfo(string type, string count) {
            Type = type;
            Count = count;
        }
    }
}
