using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Domain.Siege;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.QueryHandlers.Empires;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Empires {
    [TestClass]
    public sealed class GetSiegeQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly ResourcesMap _resourcesMap = new ResourcesMap();

        public GetSiegeQueryHandlerTests() {
            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();

            user.Id.Returns(1);
            user.Status.Returns(UserStatus.Active);
            user.Email.Returns("test@test.com");

            player.User.Returns(user);
            player.SiegeWeapons.Returns(new List<SiegeWeapon>() {
                new SiegeWeapon(player, SiegeWeaponType.FireArrows) { Count = 2 },
                new SiegeWeapon(player, SiegeWeaponType.BatteringRams) { Count = 3 },
                new SiegeWeapon(player, SiegeWeaponType.ScalingLadders) { Count = 4 }
            });
            player.GetBuildingBonus(BuildingType.SiegeFactory).Returns(6);
            player.SiegeEngineers.Returns(14);

            _context.Users.Add(user);
            _context.Players.Add(player);
        }

        [TestMethod]
        public void GetSiegeQueryHandler_Returns_Correct_FireArrows() {
            var handler = new GetSiegeQueryHandler(_context, _resourcesMap);
            var query = new GetSiegeQuery("test@test.com");

            var result = handler.Execute(query);

            result.FireArrows.Should().NotBeNull();
            result.FireArrows.Cost.Ore.Should().Be(40);
            result.FireArrows.Cost.Wood.Should().Be(80);
            result.FireArrows.TroopCount.Should().Be(36);
            result.FireArrows.Maintenance.Should().Be(18);
            result.FireArrows.TroopType.Should().Be("Archers");
            result.FireArrows.CurrentCount.Should().Be(2);
            result.FireArrows.CurrentMaintenance.Should().Be(36);
            result.FireArrows.CurrentTroopCount.Should().Be(72);
            result.FireArrows.Name.Should().Be("Fire arrows");
        }

        [TestMethod]
        public void GetSiegeQueryHandler_Returns_Correct_BatteringRams() {
            var handler = new GetSiegeQueryHandler(_context, _resourcesMap);
            var query = new GetSiegeQuery("test@test.com");

            var result = handler.Execute(query);

            result.BatteringRams.Should().NotBeNull();
            result.BatteringRams.Cost.Ore.Should().Be(100);
            result.BatteringRams.Cost.Wood.Should().Be(200);
            result.BatteringRams.TroopCount.Should().Be(8);
            result.BatteringRams.Maintenance.Should().Be(4);
            result.BatteringRams.TroopType.Should().Be("Cavalry");
            result.BatteringRams.CurrentCount.Should().Be(3);
            result.BatteringRams.CurrentMaintenance.Should().Be(12);
            result.BatteringRams.CurrentTroopCount.Should().Be(24);
            result.BatteringRams.Name.Should().Be("Battering rams");
        }

        [TestMethod]
        public void GetSiegeQueryHandler_Returns_Correct_ScalingLadders() {
            var handler = new GetSiegeQueryHandler(_context, _resourcesMap);
            var query = new GetSiegeQuery("test@test.com");

            var result = handler.Execute(query);

            result.ScalingLadders.Should().NotBeNull();
            result.ScalingLadders.Cost.Ore.Should().Be(45);
            result.ScalingLadders.Cost.Wood.Should().Be(90);
            result.ScalingLadders.TroopCount.Should().Be(12);
            result.ScalingLadders.Maintenance.Should().Be(6);
            result.ScalingLadders.TroopType.Should().Be("Footmen");
            result.ScalingLadders.CurrentCount.Should().Be(4);
            result.ScalingLadders.CurrentMaintenance.Should().Be(24);
            result.ScalingLadders.CurrentTroopCount.Should().Be(48);
            result.ScalingLadders.Name.Should().Be("Scaling ladders");
        }

        [TestMethod]
        public void GetSiegeQueryHandler_Returns_Correct_General_Information() {
            var handler = new GetSiegeQueryHandler(_context, _resourcesMap);
            var query = new GetSiegeQuery("test@test.com");

            var result = handler.Execute(query);

            result.Engineers.Should().Be(14);
            result.TotalMaintenance.Should().Be(14 * 6);
            result.AvailableMaintenance.Should().Be(14 * 6 - 24 - 12 - 36);
        }
    }
}