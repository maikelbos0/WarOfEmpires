using WarOfEmpires.Configuration;

namespace WarOfEmpires.Services {
    public sealed class ResourceService : IResourceService {
        private readonly ResourceSettings _resourceSettings;

        public ResourceService(ResourceSettings resourceSettings) {
            _resourceSettings = resourceSettings;
        }

        public string ResolveStaticResource(string relativePath) {
            if (_resourceSettings.UseCdn) {
                return $"{_resourceSettings.CdnBaseUrl.TrimEnd('/')}{relativePath.TrimStart('~')}";
            }

            return relativePath;
        }

        public string ResolveUserResource(string fileName) {
            return $"{_resourceSettings.UserResourceBaseUrl.TrimEnd('/')}/{fileName}";
        }
    }
}
