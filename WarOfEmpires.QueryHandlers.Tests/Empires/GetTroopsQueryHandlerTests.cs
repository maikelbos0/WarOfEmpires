﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.QueryHandlers.Empires;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Empires {
    [TestClass]
    public sealed class GetTroopsQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        public GetTroopsQueryHandlerTests() {
            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();

            user.Id.Returns(1);
            user.Status.Returns(UserStatus.Active);
            user.Email.Returns("test@test.com");

            player.User.Returns(user);
            player.Id.Returns(1);
            player.Peasants.Returns(1);
            player.GetTroops(TroopType.Archers).Returns(new Troops(TroopType.Archers, 7, 6));
            player.GetTroops(TroopType.Cavalry).Returns(new Troops(TroopType.Cavalry, 5, 4));
            player.GetTroops(TroopType.Footmen).Returns(new Troops(TroopType.Footmen, 3, 2));
            player.GetUpkeepPerTurn().Returns(new Resources(gold: 500, food: 30));
            player.GetTotalResources().Returns(new Resources(gold: 1000, food: 100));
            player.GetResourcesPerTurn().Returns(new Resources(gold: 400, food: 20));
            player.GetSoldierRecruitsPenalty().Returns(1);
            player.HasUpkeepRunOut.Returns(true);
            player.Stamina.Returns(100);

            _context.Users.Add(user);
            _context.Players.Add(player);
        }

        [TestMethod]
        public void GetTroopsQueryHandler_Returns_Correct_Peasants() {
            var query = new GetTroopsQuery("test@test.com");
            var handler = new GetTroopsQueryHandler(_context, new ResourcesMap());

            var result = handler.Execute(query);

            result.CurrentPeasants.Should().Be(1);
        }

        [TestMethod]
        public void GetTroopsQueryHandler_Returns_Correct_Troops() {
            var query = new GetTroopsQuery("test@test.com");
            var handler = new GetTroopsQueryHandler(_context, new ResourcesMap());

            var result = handler.Execute(query);

            result.ArcherInfo.CurrentSoldiers.Should().Be(7);
            result.ArcherInfo.CurrentMercenaries.Should().Be(6);
            result.CavalryInfo.CurrentSoldiers.Should().Be(5);
            result.CavalryInfo.CurrentMercenaries.Should().Be(4);
            result.FootmanInfo.CurrentSoldiers.Should().Be(3);
            result.FootmanInfo.CurrentMercenaries.Should().Be(2);
        }

        [TestMethod]
        public void GetTroopsQueryHandler_Returns_Correct_Additional_Information() {
            var query = new GetTroopsQuery("test@test.com");
            var handler = new GetTroopsQueryHandler(_context, new ResourcesMap());

            var result = handler.Execute(query);

            result.ArcherInfo.Cost.Gold.Should().Be(5000);
            result.ArcherInfo.Cost.Wood.Should().Be(1000);
            result.ArcherInfo.Cost.Ore.Should().Be(500);
            result.CavalryInfo.Cost.Gold.Should().Be(5000);
            result.CavalryInfo.Cost.Wood.Should().Be(0);
            result.CavalryInfo.Cost.Ore.Should().Be(1500);
            result.FootmanInfo.Cost.Gold.Should().Be(5000);
            result.FootmanInfo.Cost.Wood.Should().Be(500);
            result.FootmanInfo.Cost.Ore.Should().Be(1000);
            result.MercenaryTrainingCost.Gold.Should().Be(5000);
            result.WillUpkeepRunOut.Should().BeTrue();
            result.HasUpkeepRunOut.Should().BeTrue();
            result.HasSoldierShortage.Should().BeTrue();
        }

        [TestMethod]
        public void GetTroopsQueryHandler_Returns_Correct_Stamina() {
            var query = new GetTroopsQuery("test@test.com");
            var handler = new GetTroopsQueryHandler(_context, new ResourcesMap());
            var result = handler.Execute(query);
            result.CurrentStamina.Should().Be(100);
        }

        // TODO add tests for troop strength if/when implemented

        [TestMethod]
        public void GetTroopsQueryHandler_Returns_Correct_StaminaToHeal() {
            var query = new GetTroopsQuery("test@test.com");
            var handler = new GetTroopsQueryHandler(_context, new ResourcesMap());
            var result = handler.Execute(query);
            result.StaminaToHeal.Should().Be("0");
        }
    }
}