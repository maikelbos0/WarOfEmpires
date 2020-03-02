namespace WarOfEmpires.Domain.Empires {
    public sealed class Workers : Entity {
        public WorkerType Type { get; private set; }
        public int Count { get; set; }
        
        private Workers() { }

        public Workers(WorkerType type, int count) {
            Type = type;
            Count = count;
        }
    }
}