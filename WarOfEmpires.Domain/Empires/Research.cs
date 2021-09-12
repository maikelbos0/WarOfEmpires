namespace WarOfEmpires.Domain.Empires {
    public class Research : Entity {
        public virtual ResearchType Type { get; protected set; }
        public virtual int Level { get; set; } = 0;

        protected Research() { }

        public Research(ResearchType type) {
            Type = type;
        }
    }
}
