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
                new SiegeWeapon(SiegeWeaponType.FireArrows, 2),
                new SiegeWeapon(SiegeWeaponType.BatteringRams, 3),
                new SiegeWeapon(SiegeWeaponType.ScalingLadders, 4)
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

            result.FireArrowsInfo.Should().NotBeNull();
            result.FireArrowsInfo.Cost.Ore.Should().Be(40);
            result.FireArrowsInfo.Cost.Wood.Should().Be(80);
            result.FireArrowsInfo.TroopCount.Should().Be(36);
            result.FireArrowsInfo.Maintenance.Should().Be(18);
            result.FireArrowsInfo.CurrentCount.Should().Be(2);
            result.FireArrowsInfo.CurrentTroopCount.Should().Be(72);
            result.FireArrowsInfo.Name.Should().Be("Fire arrows");
        }

        [TestMethod]
        public void GetSiegeQueryHandler_Returns_Correct_BatteringRams() {
            var handler = new GetSiegeQueryHandler(_context, _resourcesMap);
            var query = new GetSiegeQuery("test@test.com");

            var result = handler.Execute(query);

            result.BatteringRamsInfo.Should().NotBeNull();
            result.BatteringRamsInfo.Cost.Ore.Should().Be(100);
            result.BatteringRamsInfo.Cost.Wood.Should().Be(200);
            result.BatteringRamsInfo.TroopCount.Should().Be(8);
            result.BatteringRamsInfo.Maintenance.Should().Be(4);
            result.BatteringRamsInfo.CurrentCount.Should().Be(3);
            result.BatteringRamsInfo.CurrentTroopCount.Should().Be(24);
            result.BatteringRamsInfo.Name.Should().Be("Battering rams");
        }

        [TestMethod]
        public void GetSiegeQueryHandler_Returns_Correct_ScalingLadders() {
            var handler = new GetSiegeQueryHandler(_context, _resourcesMap);
            var query = new GetSiegeQuery("test@test.com");

            var result = handler.Execute(query);

            result.ScalingLaddersInfo.Should().NotBeNull();
            result.ScalingLaddersInfo.Cost.Ore.Should().Be(45);
            result.ScalingLaddersInfo.Cost.Wood.Should().Be(90);
            result.ScalingLaddersInfo.TroopCount.Should().Be(12);
            result.ScalingLaddersInfo.Maintenance.Should().Be(6);
            result.ScalingLaddersInfo.CurrentCount.Should().Be(4);
            result.ScalingLaddersInfo.CurrentTroopCount.Should().Be(48);
            result.ScalingLaddersInfo.Name.Should().Be("Scaling ladders");
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