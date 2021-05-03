using System;
using System.Configuration;
using VDT.Core.DependencyInjection;

namespace WarOfEmpires.Utilities.Configuration {
    // TODO switch to json style settings
    [ScopedServiceImplementation(typeof(IAppSettings))]
    public sealed class AppSettings : IAppSettings {
        [Obsolete]
        public string this[string name] {
            get {
                return ConfigurationManager.AppSettings[name];
            }
        }
        
        public string DatabaseConnectionString { get; set; }
        public string ApplicationBaseUrl { get; set; }
        public string SendGridApiKey { get; set; }
    }
}
