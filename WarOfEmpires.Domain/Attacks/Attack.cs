using System;
using System.Collections.Generic;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Attacks {
    // TODO Add collections for defended and attacked to Player
    // TODO Subclass this for different attack types
    public class Attack : Entity {
        public const int AttackerMinimumStamina = 70;
        public const int DefenderMinimumStamina = 30;

        public virtual Player Attacker { get; protected set; }
        public virtual Player Defender { get; protected set; }
        public virtual ICollection<AttackRound> AttackRounds { get; protected set; } = new List<AttackRound>();
        public virtual AttackResult Result { get; protected set; } = AttackResult.Undefined;
        public virtual int Turns { get; protected set; }

        private Attack() { }

        public Attack(Player attacker, Player defender, int turns) {
            if (turns < 1 || turns > 10) throw new ArgumentOutOfRangeException("turns", "Turns must be between 1 and 10");

            Attacker = attacker;
            Defender = defender;
            Turns = turns;
        }

        public void Execute() {
            if (Result != AttackResult.Undefined) throw new InvalidOperationException("An attack can not be executed more than once");

            var random = new Random();
            var attackerStamina = Attacker.Stamina;
            var defenderStamina = Defender.Stamina;

            if (attackerStamina < AttackerMinimumStamina) {
                Result = AttackResult.Fatigued;
                return;
            }

            if (defenderStamina < DefenderMinimumStamina) {
                Result = AttackResult.Surrendered;
                return;
            }

            var calculatedAttackerStamina = random.Next(attackerStamina - 10, attackerStamina + 10);
            var calculatedDefenderStamina = random.Next(defenderStamina - 10, defenderStamina + 10);

            defenderStamina -= AddRound(calculatedAttackerStamina, TroopType.Archers, Attacker.GetArcherInfo(), Defender);
            attackerStamina -= AddRound(calculatedDefenderStamina, TroopType.Archers, Defender.GetArcherInfo(), Attacker);

            defenderStamina -= AddRound(calculatedAttackerStamina, TroopType.Cavalry, Attacker.GetCavalryInfo(), Defender);
            attackerStamina -= AddRound(calculatedDefenderStamina, TroopType.Cavalry, Defender.GetCavalryInfo(), Attacker);

            defenderStamina -= AddRound(calculatedAttackerStamina, TroopType.Footmen, Attacker.GetFootmanInfo(), Defender);
            attackerStamina -= AddRound(calculatedDefenderStamina, TroopType.Footmen, Defender.GetFootmanInfo(), Attacker);

            if (defenderStamina < 0) {
                defenderStamina = 0;
            }

            if (attackerStamina < 0) {
                attackerStamina = 0;
            }

            if (Attacker.Stamina - attackerStamina > Defender.Stamina - defenderStamina) {
                Result = AttackResult.Defended;
            }
            else {
                Result = AttackResult.Win;
            }

            // TODO get resources in player class?
        }

        public int AddRound(int stamina, TroopType troopType, TroopInfo attackerTroopInfo, Player defender) {
            var attackStrength = attackerTroopInfo.GetTotalAttack() * stamina * Turns / 25 / 100;

            if (attackStrength == 0) {
                return 0;
            }

            var totalDefenseStrength = defender.GetArcherInfo().GetTotalDefense()
                + defender.GetCavalryInfo().GetTotalDefense()
                + defender.GetFootmanInfo().GetTotalDefense();

            var archerDamage = defender.GetArcherInfo().GetTotalDefense() * attackStrength / totalDefenseStrength;
            var cavalryDamage = defender.GetCavalryInfo().GetTotalDefense() * attackStrength / totalDefenseStrength;
            var footmanDamage = defender.GetFootmanInfo().GetTotalDefense() * attackStrength / totalDefenseStrength;

            var archerCasualties = archerDamage / defender.GetArcherInfo().GetDefensePerSoldier();
            var cavalryCasualties = cavalryDamage / defender.GetCavalryInfo().GetDefensePerSoldier();
            var footmanCasualties = footmanDamage / defender.GetFootmanInfo().GetDefensePerSoldier();

            // Return stamina loss
            return attackStrength / totalDefenseStrength * 100;
        }
    }
}