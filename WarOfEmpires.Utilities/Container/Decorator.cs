namespace WarOfEmpires.Utilities.Container {
    public abstract class Decorator<THandler> {
        public THandler Handler { get; set; }
    }
}