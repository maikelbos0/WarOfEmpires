﻿using FluentAssertions;
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
    public sealed class GetReceivedAttacksQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private Player _defender;
        private Player _attacker1;
        private Player _attacker2;
        private int _attackId = 0;

        public GetReceivedAttacksQueryHandlerTests() {
            _defender = AddPlayer(1, "test@test.com", "Test", UserStatus.Active);
            _attacker1 = AddPlayer(2, "attacker1@test.com", "Attacker 1", UserStatus.Active);
            _attacker2 = AddPlayer(3, "attacker2@test.com", "Attacker 2", UserStatus.Inactive);
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

        public void AddAttack(Player attacker, int turns, Casualties[] attackerCasualties, Casualties[] defenderCasualties) {
            var attack = Substitute.For<Attack>();
            var rounds = new List<AttackRound>();

            attack.Id.Returns(++_attackId);
            attack.Date.Returns(new DateTime(2019, 1, 1));
            attack.Type.Returns(AttackType.Raid);
            attack.Attacker.Returns(attacker);
            attack.Defender.Returns(_defender);
            attacker.ExecutedAttacks.Add(attack);
            _defender.ReceivedAttacks.Add(attack);
            attack.Turns.Returns(turns);
            attack.Rounds.Returns(rounds);
            attack.Result.Returns(AttackResult.Won);
            attack.IsRead.Returns(true);

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
        public void GetReceivedAttacksQueryHandler_Returns_All_Received_Attacks() {
            AddAttack(_attacker1, 10, new Casualties[] { new Casualties(new Troops(0, 15), new Troops(0, 0), new Troops(0, 0)) }, new Casualties[] { new Casualties(new Troops(0, 15), new Troops(0, 0), new Troops(0, 0)) });
            AddAttack(_attacker1, 10, new Casualties[] { new Casualties(new Troops(0, 15), new Troops(0, 0), new Troops(0, 0)) }, new Casualties[] { new Casualties(new Troops(0, 15), new Troops(0, 0), new Troops(0, 0)) });
            AddAttack(_attacker2, 10, new Casualties[] { new Casualties(new Troops(0, 15), new Troops(0, 0), new Troops(0, 0)) }, new Casualties[] { new Casualties(new Troops(0, 15), new Troops(0, 0), new Troops(0, 0)) });

            var handler = new GetReceivedAttacksQueryHandler(_context);
            var query = new GetReceivedAttacksQuery("test@test.com");

            var result = handler.Execute(query);

            result.Should().HaveCount(3);
        }

        [TestMethod]
        public void GetReceivedAttacksQueryHandler_Returns_Correct_Data() {
            AddAttack(_attacker1, 7, new Casualties[] {
                new Casualties(new Troops(0, 10), new Troops(0, 9), new Troops(0, 8)),
                new Casualties(new Troops(4, 7), new Troops(3, 6), new Troops(2, 5))
            }, new Casualties[] {
                new Casualties(new Troops(0, 15), new Troops(0, 7), new Troops(0, 3)),
                new Casualties(new Troops(3, 20), new Troops(2, 5), new Troops(1, 3))
            });

            var handler = new GetReceivedAttacksQueryHandler(_context);
            var query = new GetReceivedAttacksQuery("test@test.com");

            var result = handler.Execute(query);

            result.Should().HaveCount(1);
            result.Single().Id.Should().Be(1);
            result.Single().Date.Should().Be(new DateTime(2019, 1, 1));
            result.Single().Turns.Should().Be(7);
            result.Single().Type.Should().Be("Raid");
            result.Single().Attacker.Should().Be("Attacker 1");
            result.Single().DefenderSoldierCasualties.Should().Be(6);
            result.Single().DefenderMercenaryCasualties.Should().Be(53);
            result.Single().AttackerSoldierCasualties.Should().Be(9);
            result.Single().AttackerMercenaryCasualties.Should().Be(45);
            result.Single().Result.Should().Be("Won");
            result.Single().IsRead.Should().BeTrue();
        }
    }
}