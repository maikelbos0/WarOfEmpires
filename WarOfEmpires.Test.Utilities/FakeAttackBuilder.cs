using NSubstitute;
using System;
using System.Collections.Generic;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Test.Utilities {
    public class FakeAttackBuilder : FakeBuilder {
        public Attack Attack { get; }

        public FakeAttackBuilder(IWarContext context, int id, Player attacker, Player defender, AttackType type, AttackResult result, int turns, bool isRead, DateTime? date, Resources resources) : base(context) {
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

            attacker.ExecutedAttacks.Add(Attack);
            defender.ReceivedAttacks.Add(Attack);
        }

        public FakeAttackBuilder AddRound(bool isAggressor, int troops, long damage, TroopType troopType, params Casualties[] casualties) {
            var round = Substitute.For<AttackRound>();

            round.IsAggressor.Returns(isAggressor);
            round.Damage.Returns(damage);
            round.TroopType.Returns(troopType);
            round.Troops.Returns(troops);
            round.Casualties.Returns(casualties);
            Attack.Rounds.Add(round);

            return this;
        }
    }
}