using System;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Attacks {
    public static class AttackFactory {
        public static Attack Get(AttackType type, Player attacker, Player defender, int turns) {
            switch (type) {
                case AttackType.Raid:
                    return new Raid(attacker, defender, turns);
                case AttackType.Assault:
                    return new Assault(attacker, defender, turns);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}