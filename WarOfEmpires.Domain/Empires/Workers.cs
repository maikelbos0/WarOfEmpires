namespace WarOfEmpires.Domain.Empires {
    public class Workers : Entity {
        public virtual WorkerType Type { get; private set; }
        public virtual int Count { get; set; }
        
        private Workers() { }

        public Workers(WorkerType type, int count) {
            Type = type;
            Count = count;
        }
    }
}