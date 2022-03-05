using System;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Attacks {
    public static class AttackFactory {
        public static Attack Get(AttackType type, Player attacker, Player defender, int turns) {
            return type switch {
                AttackType.Raid => new Raid(attacker, defender, turns),
                AttackType.Assault => new Assault(attacker, defender, turns),
                AttackType.GrandOverlordAttack => new GrandOverlordAttack(attacker, defender, turns),
                AttackType.Revenge => new Revenge(attacker, defender, turns),
                _ => throw new NotImplementedException(),
            };
        }
    }
}