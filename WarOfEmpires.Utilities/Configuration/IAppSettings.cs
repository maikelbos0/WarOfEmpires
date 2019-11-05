namespace WarOfEmpires.Utilities.Configuration {
    public interface IAppSettings {
        string this[string name] { get; }
    }
}