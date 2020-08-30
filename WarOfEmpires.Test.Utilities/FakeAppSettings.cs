using WarOfEmpires.Utilities.Configuration;
using System.Collections.Generic;

namespace WarOfEmpires.Test.Utilities {
    public sealed class FakeAppSettings : IAppSettings {
        public Dictionary<string, string> Settings { get; }

        public FakeAppSettings() {
            Settings = new Dictionary<string, string>() {
                { "Application.BaseUrl", "http://localhost/" }
            };
        }

        public string this[string name] {
            get {
                return Settings[name];
            }
            set {
                Settings[name] = value;
            }
        }
    }
}