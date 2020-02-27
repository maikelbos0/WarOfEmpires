using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Utilities.Linq;

namespace WarOfEmpires.Utilities.Tests.Linq {
    [TestClass]
    public sealed class EnumerableExtensionsTests {
        [TestMethod]
        public void EnumerableExtensions_Resources_Sum_Succeeds() {
            var resources = new List<Resources>() {
                new Resources(10, 10, 10, 10, 10),
                new Resources(gold: 1),
                new Resources(food: 2),
                new Resources(wood: 3),
                new Resources(stone: 4),
                new Resources(ore: 5)
            };

            resources.Sum().Should().Be(new Resources(11, 12, 13, 14, 15));
        }

        private sealed class Test {
            public Resources Resources { get; set; }
        }

        [TestMethod]
        public void EnumerableExtensions_Source_Sum_Succeeds() {
            var values = new List<Test>() {
                new Test() { Resources = new Resources(10, 10, 10, 10, 10) },
                new Test() { Resources = new Resources(gold: 1) },
                new Test() { Resources = new Resources(food: 2) },
                new Test() { Resources = new Resources(wood: 3) },
                new Test() { Resources = new Resources(stone: 4) },
                new Test() { Resources = new Resources(ore: 5) }
            };

            values.Sum(v => v.Resources).Should().Be(new Resources(11, 12, 13, 14, 15));
        }
    }
}