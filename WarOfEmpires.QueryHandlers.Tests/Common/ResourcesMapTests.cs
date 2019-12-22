using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.QueryHandlers.Common;

namespace WarOfEmpires.QueryHandlers.Tests.Common {
    [TestClass]
    public sealed class ResourcesMapTests {
        [TestMethod]
        public void ResourcesMap_ToViewModel_Succeeds() {
            var resources = new Resources(10000, 500, 2000, 1000, 250);
            var map = new ResourcesMap();

            var result = map.ToViewModel(resources);

            result.Gold.Should().Be(10000);
            result.Food.Should().Be(500);
            result.Wood.Should().Be(2000);
            result.Stone.Should().Be(1000);
            result.Ore.Should().Be(250);
        }
    }
}