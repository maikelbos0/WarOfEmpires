using WarOfEmpires.Utilities.Container;
using System.Configuration;

namespace WarOfEmpires.Utilities.Configuration {
    [InterfaceInjectable]
    public sealed class AppSettings : IAppSettings {
        public string this[string name] {
            get {
                return ConfigurationManager.AppSettings[name];
            }
        }
    }
}
