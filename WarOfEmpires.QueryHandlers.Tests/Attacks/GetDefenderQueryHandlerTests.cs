using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Game;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.QueryHandlers.Attacks;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Attacks {
    [TestClass]
    public sealed class GetDefenderQueryHandlerTests {
        [TestMethod]
        public void GetDefenderQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .WithGameStatus(1)
                .WithPlayer(1)
                .BuildPlayer(2)
                .WithPopulation();

            var handler = new GetDefenderQueryHandler(builder.Context);
            var query = new GetDefenderQuery("test1@test.com", 2);

            var result = handler.Execute(query);

            result.DefenderId.Should().Be(2);
            result.Defender.Should().Be("Test display name 2");
            result.Population.Should().Be(49);
            result.HasWarDamage.Should().BeFalse();
            result.ValidAttackTypes.Should().BeEquivalentTo("Raid", "Assault");
        }

        [TestMethod]
        public void GetDefenderQueryHandler_Returns_HasWarDamage_For_War() {
            var builder = new FakeBuilder()
                .WithGameStatus(1)
                .BuildAlliance(1)
                .WithMember(1);

            builder.BuildAlliance(2)
                .WithMember(2)
                .WithWar(1, builder.Alliance);

            var handler = new GetDefenderQueryHandler(builder.Context);
            var query = new GetDefenderQuery("test1@test.com", 2);

            var result = handler.Execute(query);

            result.HasWarDamage.Should().BeTrue();
        }

        [TestMethod]
        public void GetDefenderQueryHandler_Returns_HasWarDamage_When_Received_War_Attack_Recently() {
            var builder = new FakeBuilder()
                .WithGameStatus(1)
                .WithPlayer(1, out var player)
                .BuildPlayer(2)
                .WithAttackOn(1, player, AttackType.Assault, AttackResult.Defended, date: DateTime.UtcNow.AddHours(-15).AddMinutes(-59), isAtWar: true);

            var handler = new GetDefenderQueryHandler(builder.Context);
            var query = new GetDefenderQuery("test1@test.com", 2);

            var result = handler.Execute(query);

            result.HasWarDamage.Should().BeTrue();
        }

        [TestMethod]
        public void GetDefenderQueryHandler_Returns_HasWarDamage_When_Received_War_Attack_Expired() {
            var builder = new FakeBuilder()
                .WithGameStatus(1)
                .WithPlayer(1, out var player)
                .BuildPlayer(2)
                .WithAttackOn(1, player, AttackType.Assault, AttackResult.Defended, date: DateTime.UtcNow.AddHours(-16).AddMinutes(-1));

            var handler = new GetDefenderQueryHandler(builder.Context);
            var query = new GetDefenderQuery("test1@test.com", 2);

            var result = handler.Execute(query);

            result.HasWarDamage.Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow(GamePhase.Truce, true, DisplayName = "Truce")]
        [DataRow(GamePhase.Active, false, DisplayName = "Active")]
        [DataRow(GamePhase.Finished, false, DisplayName = "Finished")]
        public void GetDefenderQueryHandler_Returns_Correct_IsTruce(GamePhase phase, bool expectedIsTruce) {
            var builder = new FakeBuilder()
                .WithGameStatus(1, phase: phase)
                .WithPlayer(1)
                .WithPlayer(2);

            var handler = new GetDefenderQueryHandler(builder.Context);
            var query = new GetDefenderQuery("test1@test.com", 2);

            var result = handler.Execute(query);

            result.IsTruce.Should().Be(expectedIsTruce);
        }

        [TestMethod]
        public void GetDefenderQueryHandler_Throws_Exception_For_Nonexistent_Id() {
            var builder = new FakeBuilder()
                .WithGameStatus(1)
                .WithPlayer(1)
                .WithPlayer(2);

            var handler = new GetDefenderQueryHandler(builder.Context);
            var query = new GetDefenderQuery("test1@test.com", 5);

            Action action = () => handler.Execute(query);

            action.Should().Throw<InvalidOperationException>();
        }

        [DataTestMethod]
        [DataRow(TitleType.Overlord, TitleType.Overlord, false, DisplayName = "Overlord against Overlord")]
        [DataRow(TitleType.Emperor, TitleType.GrandOverlord, false, DisplayName = "Emperor against Grand Overlord")]
        [DataRow(TitleType.Overlord, TitleType.GrandOverlord, true, DisplayName = "Overlord against Grand Overlord")]
        [DataRow(TitleType.GrandOverlord, TitleType.Overlord, false, DisplayName = "Grand Overlord against Overlord")]
        public void GetDefenderQueryHandler_Adds_ValidAttackType_GrandOverlordAttack_Correctly(TitleType attackerTitle, TitleType defenderTitle, bool expectedGrandOverlordAttack) {
            var builder = new FakeBuilder()
                .WithGameStatus(1)
                .WithPlayer(1, title: attackerTitle)
                .WithPlayer(2, title: defenderTitle);

            var handler = new GetDefenderQueryHandler(builder.Context);
            var query = new GetDefenderQuery("test1@test.com", 2);

            var result = handler.Execute(query);

            if (expectedGrandOverlordAttack) {
                result.ValidAttackTypes.Should().Contain("GrandOverlordAttack");
            }
            else {
                result.ValidAttackTypes.Should().NotContain("GrandOverlordAttack");
            }
        }

        [DataTestMethod]
        [DataRow(null, null, false, DisplayName = "No received attacks, no executed revenge")]
        [DataRow(16 * 60 + 1, null, false, DisplayName = "Received attack too long ago, no executed revenge")]
        [DataRow(16 * 60 - 1, null, true, DisplayName = "Valid received attack, no executed revenge")]
        [DataRow(12 * 60, 11 * 60, false, DisplayName = "Valid received attack, executed revenge later")]
        [DataRow(12 * 60, 13 * 60, true, DisplayName = "Valid received attack, executed revenge earlier")]
        public void GetDefenderQueryHandler_Adds_ValidAttackType_Revenge_Correctly(int? minutesSinceLastReceivedAttack, int? minutesSinceLastExecutedRevenge, bool expectedRevenge) {
            var attackerBuilder = new FakeBuilder()
                .WithGameStatus(1)
                .BuildPlayer(1);
            var defenderBuilder = attackerBuilder
                .BuildPlayer(2);

            if (minutesSinceLastReceivedAttack.HasValue) {
                defenderBuilder
                    .WithAttackOn(2, attackerBuilder.Player, AttackType.Raid, AttackResult.Won, date: DateTime.UtcNow.AddMinutes(-minutesSinceLastReceivedAttack.Value));
            }

            attackerBuilder
                .WithAttackOn(1, attackerBuilder.Player, AttackType.Raid, AttackResult.Won, date: DateTime.UtcNow.AddMinutes(-1));

            if (minutesSinceLastExecutedRevenge.HasValue) {
                attackerBuilder
                    .WithAttackOn(3, defenderBuilder.Player, AttackType.Revenge, AttackResult.Won, date: DateTime.UtcNow.AddMinutes(-minutesSinceLastExecutedRevenge.Value));
            }

            var handler = new GetDefenderQueryHandler(attackerBuilder.Context);
            var query = new GetDefenderQuery("test1@test.com", 2);

            var result = handler.Execute(query);

            if (expectedRevenge) {
                result.ValidAttackTypes.Should().Contain("Revenge");
            }
            else {
                result.ValidAttackTypes.Should().NotContain("Revenge");
            }
        }
    }
}