using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Services;
using WarOfEmpires.Utilities.Configuration;

namespace WarOfEmpires.Tests.Services {
    [TestClass]
    public sealed class StaticResourceServiceTests {
        [TestMethod]
        public void StaticResourceService_Resolve_Succeeds_Without_Cdn() {
            var appSettings = new AppSettings() {
                UseCdn = false
            };
            var service = new StaticResourceService(appSettings);

            service.Resolve("~/script/test.js").Should().Be("~/script/test.js");
        }

        [DataTestMethod]
        [DataRow("https://localhost/")]
        [DataRow("https://localhost")]
        public void StaticResourceService_Resolve_Succeeds_With_Cdn(string cdnBaseUrl) {
            var appSettings = new AppSettings() {
                UseCdn = true,
                CdnBaseUrl = cdnBaseUrl
            };
            var service = new StaticResourceService(appSettings);

            service.Resolve("~/script/test.js").Should().Be("https://localhost/script/test.js");
        }
    }
}
