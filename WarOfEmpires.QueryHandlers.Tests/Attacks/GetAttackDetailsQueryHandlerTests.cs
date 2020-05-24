using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.QueryHandlers.Attacks;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Tests.Attacks {
    [TestClass]
    public sealed class GetAttackDetailsQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly ResourcesMap _resourceMap = new ResourcesMap();
        private readonly EnumFormatter _formatter = new EnumFormatter();
        private readonly Player _attacker;
        private readonly Player _defender;

        public GetAttackDetailsQueryHandlerTests() {
            _attacker = AddPlayer(1, "attacker@test.com", "Attacker 1");
            _defender = AddPlayer(2, "defender@test.com", "Defender 1");
            AddPlayer(3, "random@test.com", "Random");

            var attackerAlliance = Substitute.For<Alliance>();
            var defenderAlliance = Substitute.For<Alliance>();
            var attack = Substitute.For<Attack>();
            var rounds = new List<AttackRound>();

            _attacker.Alliance.Returns(attackerAlliance);
            attackerAlliance.Id.Returns(27);
            attackerAlliance.Code.Returns("ATK");
            attackerAlliance.Name.Returns("The Attackers");

            _defender.Alliance.Returns(defenderAlliance);
            defenderAlliance.Id.Returns(14);
            defenderAlliance.Code.Returns("DEF");
            defenderAlliance.Name.Returns("The Defenders");

            attack.Id.Returns(1);
            attack.Date.Returns(new DateTime(2019, 1, 1));
            attack.IsRead.Returns(false);
            attack.Type.Returns(AttackType.Raid);
            attack.Attacker.Returns(_attacker);
            attack.Defender.Returns(_defender);
            _attacker.ExecutedAttacks.Add(attack);
            _defender.ReceivedAttacks.Add(attack);
            attack.Turns.Returns(10);
            attack.Rounds.Returns(rounds);
            attack.Result.Returns(AttackResult.Won);
            attack.Resources.Returns(new Resources(1, 2, 3, 4, 5));

            rounds.Add(CreateRound(true, 200, 17000, TroopType.Archers, new Casualties(TroopType.Archers, 0, 15), new Casualties(TroopType.Cavalry, 0, 14), new Casualties(TroopType.Footmen, 0, 13)));
            rounds.Add(CreateRound(false, 150, 16000, TroopType.Archers, new Casualties(TroopType.Archers, 0, 12), new Casualties(TroopType.Cavalry, 0, 11), new Casualties(TroopType.Footmen, 0, 10)));
            rounds.Add(CreateRound(false, 170, 15000, TroopType.Cavalry, new Casualties(TroopType.Archers, 1, 9), new Casualties(TroopType.Cavalry, 2, 8), new Casualties(TroopType.Footmen, 3, 7)));
            rounds.Add(CreateRound(true, 130, 14000, TroopType.Footmen, new Casualties(TroopType.Archers, 4, 6), new Casualties(TroopType.Cavalry, 5, 5), new Casualties(TroopType.Footmen, 6, 4)));
        }

        public AttackRound CreateRound(bool isAggressor, int troops, long damage, TroopType troopType, params Casualties[] casualties) {
            var round = Substitute.For<AttackRound>();

            round.IsAggressor.Returns(isAggressor);
            round.Damage.Returns(damage);
            round.TroopType.Returns(troopType);
            round.Troops.Returns(troops);
            round.Casualties.Returns(casualties);

            return round;
        }

        public Player AddPlayer(int id, string email, string displayName) {
            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();

            user.Id.Returns(id);
            user.Status.Returns(UserStatus.Active);
            user.Email.Returns(email);

            player.User.Returns(user);
            player.Id.Returns(id);
            player.DisplayName.Returns(displayName);
            player.ExecutedAttacks.Returns(new List<Attack>());
            player.ReceivedAttacks.Returns(new List<Attack>());

            _context.Users.Add(user);
            _context.Players.Add(player);

            return player;
        }

        [TestMethod]
        public void GetAttackDetailsQueryHandler_Returns_Correct_Information() {
            var handler = new GetAttackDetailsQueryHandler(_context, _resourceMap, _formatter);
            var query = new GetAttackDetailsQuery("defender@test.com", "1");

            var result = handler.Execute(query);

            result.Id.Should().Be(1);
            result.AttackerId.Should().Be(1);
            result.Attacker.Should().Be("Attacker 1");
            result.AttackerAllianceId.Should().Be(27);
            result.AttackerAllianceCode.Should().Be("ATK");
            result.AttackerAllianceName.Should().Be("The Attackers");
            result.DefenderId.Should().Be(2);
            result.Defender.Should().Be("Defender 1");
            result.DefenderAllianceId.Should().Be(14);
            result.DefenderAllianceCode.Should().Be("DEF");
            result.DefenderAllianceName.Should().Be("The Defenders");
            result.Date.Should().Be(new DateTime(2019, 1, 1));
            result.IsRead.Should().BeFalse();
            result.Turns.Should().Be(10);
            result.Type.Should().Be("Raid");
            result.Result.Should().Be("Won");
            result.Resources.Gold.Should().Be(1);
            result.Resources.Food.Should().Be(2);
            result.Resources.Wood.Should().Be(3);
            result.Resources.Stone.Should().Be(4);
            result.Resources.Ore.Should().Be(5);

            result.Rounds.Should().HaveCount(4);
            result.Rounds[2].IsAggressor.Should().BeFalse();
            result.Rounds[2].Attacker.Should().Be("Defender 1");
            result.Rounds[2].Defender.Should().Be("Attacker 1");
            result.Rounds[2].TroopType.Should().Be("Cavalry");
            result.Rounds[2].Troops.Should().Be(170);
            result.Rounds[2].Damage.Should().Be(15000);
            result.Rounds[2].SoldierCasualties.Should().Be(6);
            result.Rounds[2].MercenaryCasualties.Should().Be(24);
        }

        [TestMethod]
        public void GetAttackDetailsQueryHandler_Succeeds_For_Defender() {
            var handler = new GetAttackDetailsQueryHandler(_context, _resourceMap, _formatter);
            var query = new GetAttackDetailsQuery("defender@test.com", "1");

            var result = handler.Execute(query);

            result.Should().NotBeNull();
        }

        [TestMethod]
        public void GetAttackDetailsQueryHandler_Succeeds_For_Attacker() {
            var handler = new GetAttackDetailsQueryHandler(_context, _resourceMap, _formatter);
            var query = new GetAttackDetailsQuery("attacker@test.com", "1");

            var result = handler.Execute(query);

            result.Should().NotBeNull();
        }

        [TestMethod]
        public void GetAttackDetailsQueryHandler_IsRead_Is_Always_True_For_Attacker() {
            var handler = new GetAttackDetailsQueryHandler(_context, _resourceMap, _formatter);
            var query = new GetAttackDetailsQuery("attacker@test.com", "1");

            var result = handler.Execute(query);

            result.IsRead.Should().BeTrue();
        }

        [TestMethod]
        public void GetAttackDetailsQueryHandler_Throws_Exception_For_Alphanumeric_AttackId() {
            var handler = new GetAttackDetailsQueryHandler(_context, _resourceMap, _formatter);
            var query = new GetAttackDetailsQuery("attacker@test.com", "A");

            Action action = () => handler.Execute(query);

            action.Should().Throw<FormatException>();
        }

        [TestMethod]
        public void GetAttackDetailsQueryHandler_Throws_Exception_For_Attack_On_And_By_Different_Player() {
            var handler = new GetAttackDetailsQueryHandler(_context, _resourceMap, _formatter);
            var query = new GetAttackDetailsQuery("random@test.com", "1");

            Action action = () => handler.Execute(query);

            action.Should().Throw<InvalidOperationException>();
        }
    }
}