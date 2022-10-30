using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Configuration;
using WarOfEmpires.Services;

namespace WarOfEmpires.Tests.Services {
    [TestClass]
    public sealed class ResourceServiceTests {
        [TestMethod]
        public void ResourceService_ResolveStaticResource_Succeeds_Without_Cdn() {
            var resourceSettings = new ResourceSettings() {
                UseCdn = false
            };
            var service = new ResourceService(resourceSettings);

            service.ResolveStaticResource("~/script/test.js").Should().Be("~/script/test.js");
        }

        [DataTestMethod]
        [DataRow("https://localhost/")]
        [DataRow("https://localhost")]
        public void ResourceService_ResolveStaticResource_Succeeds_With_Cdn(string cdnBaseUrl) {
            var resourceSettings = new ResourceSettings() {
                UseCdn = true,
                CdnBaseUrl = cdnBaseUrl
            };
            var service = new ResourceService(resourceSettings);

            service.ResolveStaticResource("~/script/test.js").Should().Be("https://localhost/script/test.js");
        }

        [DataTestMethod]
        [DataRow("https://localhost/")]
        [DataRow("https://localhost")]
        public void ResourceService_ResolveUserResource_Succeeds(string userResourceBaseUrl) {
            var resourceSettings = new ResourceSettings() {
                UserResourceBaseUrl = userResourceBaseUrl
            };
            var service = new ResourceService(resourceSettings);

            service.ResolveUserResource("image.png").Should().Be("https://localhost/image.png");
        }
    }
}
