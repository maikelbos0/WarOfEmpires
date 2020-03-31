using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
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
                new SiegeWeapon(SiegeWeaponType.FireArrows, 2),
                new SiegeWeapon(SiegeWeaponType.BatteringRams, 3),
                new SiegeWeapon(SiegeWeaponType.ScalingLadders, 4)
            });
            player.GetBuildingBonus(BuildingType.SiegeFactory).Returns(6);
            player.Workers.Returns(new List<Workers>() {
                new Workers(WorkerType.SiegeEngineers, 14)
            });

            _context.Users.Add(user);
            _context.Players.Add(player);
        }
        
        [TestMethod]
        public void GetSiegeQueryHandler_Returns_Correct_FireArrows() {
            var handler = new GetSiegeQueryHandler(_context, _resourcesMap);
            var query = new GetSiegeQuery("test@test.com");

            var result = handler.Execute(query);
            var siegeWeapon = result.SiegeWeapons.Single(s => s.Type == "FireArrows");

            siegeWeapon.Cost.Ore.Should().Be(40);
            siegeWeapon.Cost.Wood.Should().Be(80);
            siegeWeapon.TroopCount.Should().Be(36);
            siegeWeapon.Maintenance.Should().Be(18);
            siegeWeapon.CurrentCount.Should().Be(2);
            siegeWeapon.CurrentTroopCount.Should().Be(72);
            siegeWeapon.Name.Should().Be("Fire arrows");
        }
        
        [TestMethod]
        public void GetSiegeQueryHandler_Returns_Correct_BatteringRams() {
            var handler = new GetSiegeQueryHandler(_context, _resourcesMap);
            var query = new GetSiegeQuery("test@test.com");

            var result = handler.Execute(query);
            var siegeWeapon = result.SiegeWeapons.Single(s => s.Type == "BatteringRams");

            siegeWeapon.Cost.Ore.Should().Be(100);
            siegeWeapon.Cost.Wood.Should().Be(200);
            siegeWeapon.TroopCount.Should().Be(8);
            siegeWeapon.Maintenance.Should().Be(4);
            siegeWeapon.CurrentCount.Should().Be(3);
            siegeWeapon.CurrentTroopCount.Should().Be(24);
            siegeWeapon.Name.Should().Be("Battering rams");
        }

        [TestMethod]
        public void GetSiegeQueryHandler_Returns_Correct_ScalingLadders() {
            var handler = new GetSiegeQueryHandler(_context, _resourcesMap);
            var query = new GetSiegeQuery("test@test.com");

            var result = handler.Execute(query);
            var siegeWeapon = result.SiegeWeapons.Single(s => s.Type == "ScalingLadders");

            siegeWeapon.Should().NotBeNull();
            siegeWeapon.Cost.Ore.Should().Be(45);
            siegeWeapon.Cost.Wood.Should().Be(90);
            siegeWeapon.TroopCount.Should().Be(12);
            siegeWeapon.Maintenance.Should().Be(6);
            siegeWeapon.CurrentCount.Should().Be(4);
            siegeWeapon.CurrentTroopCount.Should().Be(48);
            siegeWeapon.Name.Should().Be("Scaling ladders");
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