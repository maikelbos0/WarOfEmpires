using WarOfEmpires.Utilities.Configuration;

namespace WarOfEmpires.Services {
    public sealed class StaticResourceService : IStaticResourceService {
        private readonly AppSettings _appSettings;

        public StaticResourceService(AppSettings appSettings) {
            _appSettings = appSettings;
        }

        public string Resolve(string relativePath) {
            if (_appSettings.UseCdn) {
                return $"{_appSettings.CdnBaseUrl.TrimEnd('/')}{relativePath.TrimStart('~')}";
            }

            return relativePath;
        }
    }
}
