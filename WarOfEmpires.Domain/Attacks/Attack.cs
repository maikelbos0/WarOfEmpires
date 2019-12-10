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
        public virtual ICollection<AttackRound> Rounds { get; protected set; } = new List<AttackRound>();
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

            defenderStamina -= AddRound(calculatedAttackerStamina, TroopType.Archers, true, Attacker.GetArcherInfo(), Defender);
            attackerStamina -= AddRound(calculatedDefenderStamina, TroopType.Archers, false, Defender.GetArcherInfo(), Attacker);

            defenderStamina -= AddRound(calculatedAttackerStamina, TroopType.Cavalry, true, Attacker.GetCavalryInfo(), Defender);
            attackerStamina -= AddRound(calculatedDefenderStamina, TroopType.Cavalry, false, Defender.GetCavalryInfo(), Attacker);

            defenderStamina -= AddRound(calculatedAttackerStamina, TroopType.Footmen, true, Attacker.GetFootmanInfo(), Defender);
            attackerStamina -= AddRound(calculatedDefenderStamina, TroopType.Footmen, false, Defender.GetFootmanInfo(), Attacker);
            
            if (Attacker.Stamina - attackerStamina > Defender.Stamina - defenderStamina) {
                Result = AttackResult.Defended;
            }
            else {
                Result = AttackResult.Win;
            }

            // Defender.Stamina = Math.Max(defenderStamina, 0);
            // Attacker.Stamina = Math.Max(attackerStamina, 0);

            // TODO get resources in player class?
        }

        public int AddRound(int stamina, TroopType troopType, bool isAggressor, TroopInfo attackerTroopInfo, Player defender) {
            var damage = attackerTroopInfo.GetTotalAttack() * stamina * Turns / 100 / 100;

            if (damage == 0) {
                return 0;
            }

            var totalDefense = defender.GetArcherInfo().GetTotalDefense() + defender.GetCavalryInfo().GetTotalDefense() + defender.GetFootmanInfo().GetTotalDefense();
            var archerDamage = defender.GetArcherInfo().GetTotalDefense() * damage / totalDefense;
            var cavalryDamage = defender.GetCavalryInfo().GetTotalDefense() * damage / totalDefense;
            var footmanDamage = defender.GetFootmanInfo().GetTotalDefense() * damage / totalDefense;
            
            // TODO Add stamina drain to kill
            // TODO move damage calculation to defender?

            Rounds.Add(new AttackRound(
                this,
                troopType,
                isAggressor,
                attackerTroopInfo.Soldiers + attackerTroopInfo.Mercenaries,
                damage,
                defender.KillArchers(archerDamage),
                defender.KillCavalry(cavalryDamage),
                defender.KillFootmen(footmanDamage)
            ));

            // Return stamina loss
            return damage / totalDefense * 100;
        }
    }
}