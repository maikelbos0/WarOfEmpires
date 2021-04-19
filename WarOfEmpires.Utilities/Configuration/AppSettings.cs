using System.Configuration;
using VDT.Core.DependencyInjection;

namespace WarOfEmpires.Utilities.Configuration {
    // TODO switch to json style settings
    [ScopedServiceImplementation(typeof(IAppSettings))]
    public sealed class AppSettings : IAppSettings {
        public string this[string name] {
            get {
                return ConfigurationManager.AppSettings[name];
            }
        }
    }
}
