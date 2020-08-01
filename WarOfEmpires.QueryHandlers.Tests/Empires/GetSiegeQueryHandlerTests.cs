using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Siege;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.QueryHandlers.Empires;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Tests.Empires {
    [TestClass]
    public sealed class GetSiegeQueryHandlerTests {
        [TestMethod]
        public void GetSiegeQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder().BuildPlayer(1)
                .WithBuilding(BuildingType.SiegeFactory, 6)
                .WithWorkers(WorkerType.SiegeEngineers, 14)
                .WithSiege(SiegeWeaponType.FireArrows, 2)
                .WithSiege(SiegeWeaponType.BatteringRams, 3)
                .WithSiege(SiegeWeaponType.ScalingLadders, 4);

            var handler = new GetSiegeQueryHandler(builder.Context, new ResourcesMap(), new EnumFormatter());
            var query = new GetSiegeQuery("test1@test.com");

            var result = handler.Execute(query);
            
            result.Engineers.Should().Be(14);
            result.TotalMaintenance.Should().Be(14 * 6);
            result.AvailableMaintenance.Should().Be(14 * 6 - 24 - 12 - 36);
            result.SiegeWeapons.Should().HaveCount(3);

            var batteringRams = result.SiegeWeapons.Single(s => s.Type == "BatteringRams");

            batteringRams.Cost.Ore.Should().Be(100);
            batteringRams.Cost.Wood.Should().Be(200);
            batteringRams.TroopCount.Should().Be(8);
            batteringRams.Maintenance.Should().Be(4);
            batteringRams.CurrentCount.Should().Be(3);
            batteringRams.CurrentTroopCount.Should().Be(24);
            batteringRams.Name.Should().Be("Battering rams");
        }
    }
}