using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.QueryHandlers.Attacks;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Tests.Attacks {
    [TestClass]
    public sealed class GetExecutedAttacksQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly EnumFormatter _formatter = new EnumFormatter();
        private readonly Player _attacker;
        private readonly Player _defender1;
        private readonly Player _defender2;
        private int _attackId = 0;

        public GetExecutedAttacksQueryHandlerTests() {
            _attacker = AddPlayer(1, "test@test.com", "Test", UserStatus.Active);
            _defender1 = AddPlayer(2, "defender1@test.com", "Defender 1", UserStatus.Active, "DEF");
            _defender2 = AddPlayer(3, "defender2@test.com", "Defender 2", UserStatus.Inactive);
        }

        public Player AddPlayer(int id, string email, string displayName, UserStatus status, string allianceCode = null) {
            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();

            user.Id.Returns(id);
            user.Status.Returns(status);
            user.Email.Returns(email);

            player.User.Returns(user);
            player.Id.Returns(id);
            player.DisplayName.Returns(displayName);
            player.ExecutedAttacks.Returns(new List<Attack>());
            player.ReceivedAttacks.Returns(new List<Attack>());

            if (!string.IsNullOrEmpty(allianceCode)) {
                var alliance = Substitute.For<Alliance>();
                player.Alliance.Returns(alliance);
                alliance.Code.Returns(allianceCode);
            }

            _context.Users.Add(user);
            _context.Players.Add(player);

            return player;
        }

        public void AddAttack(Player defender, int turns, List<Casualties>[] attackerCasualties, List<Casualties>[] defenderCasualties) {
            var attack = Substitute.For<Attack>();
            var rounds = new List<AttackRound>();

            attack.Id.Returns(++_attackId);
            attack.Date.Returns(new DateTime(2019, 1, 1));
            attack.Type.Returns(AttackType.Raid);
            attack.Attacker.Returns(_attacker);
            attack.Defender.Returns(defender);
            _attacker.ExecutedAttacks.Add(attack);
            defender.ReceivedAttacks.Add(attack);
            attack.Turns.Returns(turns);
            attack.Rounds.Returns(rounds);
            attack.Result.Returns(AttackResult.Won);

            foreach (var casualties in attackerCasualties) {
                var round = Substitute.For<AttackRound>();

                round.IsAggressor.Returns(false);
                round.Casualties.Returns(casualties);

                rounds.Add(round);
            }

            foreach (var casualties in defenderCasualties) {
                var round = Substitute.For<AttackRound>();

                round.IsAggressor.Returns(true);
                round.Casualties.Returns(casualties);

                rounds.Add(round);
            }
        }

        [TestMethod]
        public void GetExecutedAttacksQueryHandler_Returns_All_Executed_Attacks() {
            AddAttack(_defender1, 10, new List<Casualties>[] { new List<Casualties>() { new Casualties(TroopType.Archers, 0, 15), new Casualties(TroopType.Footmen, 0, 0), new Casualties(TroopType.Cavalry, 0, 0) } }, new List<Casualties>[] { new List<Casualties>() { new Casualties(TroopType.Archers, 0, 15), new Casualties(TroopType.Footmen, 0, 0), new Casualties(TroopType.Cavalry, 0, 0) } });
            AddAttack(_defender1, 10, new List<Casualties>[] { new List<Casualties>() { new Casualties(TroopType.Archers, 0, 15), new Casualties(TroopType.Footmen, 0, 0), new Casualties(TroopType.Cavalry, 0, 0) } }, new List<Casualties>[] { new List<Casualties>() { new Casualties(TroopType.Archers, 0, 15), new Casualties(TroopType.Footmen, 0, 0), new Casualties(TroopType.Cavalry, 0, 0) } });
            AddAttack(_defender2, 10, new List<Casualties>[] { new List<Casualties>() { new Casualties(TroopType.Archers, 0, 15), new Casualties(TroopType.Footmen, 0, 0), new Casualties(TroopType.Cavalry, 0, 0) } }, new List<Casualties>[] { new List<Casualties>() { new Casualties(TroopType.Archers, 0, 15), new Casualties(TroopType.Footmen, 0, 0), new Casualties(TroopType.Cavalry, 0, 0) } });
            var handler = new GetExecutedAttacksQueryHandler(_context, _formatter);
            var query = new GetExecutedAttacksQuery("test@test.com");

            var result = handler.Execute(query);

            result.Should().HaveCount(3);
        }

        [TestMethod]
        public void GetExecutedAttacksQueryHandler_Returns_Correct_Data() {
            AddAttack(_defender1, 7, new List<Casualties>[] {
                new List<Casualties>() { new Casualties(TroopType.Archers, 0, 10), new Casualties(TroopType.Footmen, 0, 9), new Casualties(TroopType.Cavalry, 0, 8) },
                new List<Casualties>() { new Casualties(TroopType.Archers, 4, 7), new Casualties(TroopType.Footmen, 3, 6), new Casualties(TroopType.Cavalry, 2, 5) }
            }, new List<Casualties>[] {
                new List<Casualties>() { new Casualties(TroopType.Archers, 0, 15), new Casualties(TroopType.Footmen, 0, 7), new Casualties(TroopType.Cavalry, 0, 3) },
                new List<Casualties>() { new Casualties(TroopType.Archers, 3, 20), new Casualties(TroopType.Footmen, 2, 5), new Casualties(TroopType.Cavalry, 1, 3) }
            });

            var handler = new GetExecutedAttacksQueryHandler(_context, _formatter);
            var query = new GetExecutedAttacksQuery("test@test.com");

            var result = handler.Execute(query);

            result.Should().HaveCount(1);
            result.Single().Id.Should().Be(1);
            result.Single().Date.Should().Be(new DateTime(2019, 1, 1));
            result.Single().Turns.Should().Be(7);
            result.Single().Type.Should().Be("Raid");
            result.Single().Defender.Should().Be("Defender 1");
            result.Single().DefenderAlliance.Should().Be("DEF");
            result.Single().DefenderSoldierCasualties.Should().Be(6);
            result.Single().DefenderMercenaryCasualties.Should().Be(53);
            result.Single().AttackerSoldierCasualties.Should().Be(9);
            result.Single().AttackerMercenaryCasualties.Should().Be(45);
            result.Single().Result.Should().Be("Won");
        }
    }
}