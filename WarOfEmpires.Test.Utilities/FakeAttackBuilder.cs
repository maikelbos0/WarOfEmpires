using NSubstitute;
using System;
using System.Collections.Generic;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Test.Utilities {
    public class FakeAttackBuilder : FakeBuilder {
        public Attack Attack { get; }

        internal FakeAttackBuilder(FakeWarContext context, int id, Player attacker, Player defender, AttackType type, AttackResult result, int turns, bool isRead, DateTime? date, Resources resources, bool isAtWar) : base(context) {
            Attack = Substitute.For<Attack>();

            Attack.Id.Returns(id);
            Attack.Type.Returns(type);
            Attack.Result.Returns(result);
            Attack.Attacker.Returns(attacker);
            Attack.Defender.Returns(defender);
            Attack.Turns.Returns(turns);
            Attack.IsRead.Returns(isRead);
            Attack.Date.Returns(date ?? new DateTime(2019, 1, 1));
            Attack.Resources.Returns(resources ?? new Resources());
            Attack.Rounds.Returns(new List<AttackRound>());
            Attack.IsAtWar.Returns(isAtWar);

            attacker.ExecutedAttacks.Add(Attack);
            defender.ReceivedAttacks.Add(Attack);
        }

        public FakeAttackBuilder WithRound(bool isAggressor, int troops, long damage, TroopType troopType, params Casualties[] casualties) {
            var round = Substitute.For<AttackRound>();

            round.IsAggressor.Returns(isAggressor);
            round.Damage.Returns(damage);
            round.TroopType.Returns(troopType);
            round.Troops.Returns(troops);
            round.Casualties.Returns(casualties);
            Attack.Rounds.Add(round);

            return this;
        }

        public FakeAttackBuilder WithRound(bool isAggressor, params Casualties[] casualties) {
            return WithRound(isAggressor, 15, 1000, TroopType.Archers, casualties);
        }
    }
}
