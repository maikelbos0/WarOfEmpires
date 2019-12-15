using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.QueryHandlers.Attacks;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Attacks {
    [TestClass]
    public sealed class GetExecutedAttacksQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private Player _attacker;
        private Player _defender1;
        private Player _defender2;
        private int _attackId = 0;

        public GetExecutedAttacksQueryHandlerTests() {
            _attacker = AddPlayer(1, "test@test.com", "Test", UserStatus.Active);
            _defender1 = AddPlayer(2, "defender1@test.com", "Defender 1", UserStatus.Active);
            _defender2 = AddPlayer(3, "defender2@test.com", "Defender 2", UserStatus.Inactive);
        }

        public Player AddPlayer(int id, string email, string displayName, UserStatus status) {
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

            _context.Users.Add(user);
            _context.Players.Add(player);

            return player;
        }

        public void AddAttack(Player defender, int turns, Casualties[] attackerCasualties, Casualties[] defenderCasualties) {
            var attack = Substitute.For<Attack>();
            var rounds = new List<AttackRound>();

            attack.Id.Returns(++_attackId);
            attack.Date.Returns(new DateTime(2019, 1, 1));
            attack.Attacker.Returns(_attacker);
            attack.Defender.Returns(defender);
            _attacker.ExecutedAttacks.Add(attack);
            defender.ReceivedAttacks.Add(attack);
            attack.Turns.Returns(turns);
            attack.Rounds.Returns(rounds);

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
            AddAttack(_defender1, 10, new Casualties[] { new Casualties(new Troops(0, 15), new Troops(0, 0), new Troops(0, 0)) }, new Casualties[] { new Casualties(new Troops(0, 15), new Troops(0, 0), new Troops(0, 0)) });
            AddAttack(_defender1, 10, new Casualties[] { new Casualties(new Troops(0, 15), new Troops(0, 0), new Troops(0, 0)) }, new Casualties[] { new Casualties(new Troops(0, 15), new Troops(0, 0), new Troops(0, 0)) });
            AddAttack(_defender2, 10, new Casualties[] { new Casualties(new Troops(0, 15), new Troops(0, 0), new Troops(0, 0)) }, new Casualties[] { new Casualties(new Troops(0, 15), new Troops(0, 0), new Troops(0, 0)) });

            var handler = new GetExecutedAttacksQueryHandler(_context);
            var query = new GetExecutedAttacksQuery("test@test.com");

            var results = handler.Execute(query);

            results.Should().HaveCount(3);
        }

        [TestMethod]
        public void GetExecutedAttacksQueryHandler_Returns_Correct_Data() {
            AddAttack(_defender1, 7, new Casualties[] {
                new Casualties(new Troops(0, 10), new Troops(0, 9), new Troops(0, 8)),
                new Casualties(new Troops(4, 7), new Troops(3, 6), new Troops(2, 5))
            }, new Casualties[] {
                new Casualties(new Troops(0, 15), new Troops(0, 7), new Troops(0, 3)),
                new Casualties(new Troops(3, 20), new Troops(2, 5), new Troops(1, 3))
            });

            var handler = new GetExecutedAttacksQueryHandler(_context);
            var query = new GetExecutedAttacksQuery("test@test.com");

            var results = handler.Execute(query);

            results.Should().HaveCount(1);
            results.Single().Id.Should().Be(1);
            results.Single().Date.Should().Be(new DateTime(2019, 1, 1));
            results.Single().Turns.Should().Be(7);
            results.Single().Defender.Should().Be("Defender 1");
            results.Single().DefenderSoldierCasualties.Should().Be(6);
            results.Single().DefenderMercenaryCasualties.Should().Be(53);
            results.Single().AttackerSoldierCasualties.Should().Be(9);
            results.Single().AttackerMercenaryCasualties.Should().Be(45);
        }
    }
}