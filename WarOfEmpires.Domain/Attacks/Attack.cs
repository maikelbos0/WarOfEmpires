using System;
using System.Collections.Generic;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Attacks {
    // TODO Add collections for defended and attacked to Player
    // TODO Rework this for different attack types
    public class Attack : Entity {
        public const int AttackerMinimumStamina = 70;
        public const int DefenderMinimumStamina = 30;
        public const int StaminaRandomModifier = 10;

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

            var calculatedAttackerStamina = random.Next(attackerStamina - StaminaRandomModifier, attackerStamina + StaminaRandomModifier);
            var calculatedDefenderStamina = random.Next(defenderStamina - StaminaRandomModifier, defenderStamina + StaminaRandomModifier);

            AddRound(calculatedAttackerStamina, TroopType.Archers, true, Attacker.GetArcherInfo(), Defender);
            AddRound(calculatedDefenderStamina, TroopType.Archers, false, Defender.GetArcherInfo(), Attacker);

            AddRound(calculatedAttackerStamina, TroopType.Cavalry, true, Attacker.GetCavalryInfo(), Defender);
            AddRound(calculatedDefenderStamina, TroopType.Cavalry, false, Defender.GetCavalryInfo(), Attacker);

            AddRound(calculatedAttackerStamina, TroopType.Footmen, true, Attacker.GetFootmanInfo(), Defender);
            AddRound(calculatedDefenderStamina, TroopType.Footmen, false, Defender.GetFootmanInfo(), Attacker);
            
            if (Attacker.Stamina - attackerStamina > Defender.Stamina - defenderStamina) {
                Result = AttackResult.Won;
            }
            else {
                Result = AttackResult.Defended;
            }

            // TODO Transfer resources
        }

        public void AddRound(int stamina, TroopType troopType, bool isAggressor, TroopInfo attackerTroopInfo, Player defender) {
            // We first multiply and only last divide to get the most accurate values without resorting to decimals
            var damage = attackerTroopInfo.GetTotalAttack() * stamina * Turns / 100; 

            if (damage == 0) {
                return;
            }
            
            Rounds.Add(new AttackRound(
                this,
                troopType,
                isAggressor,
                attackerTroopInfo.Troops.GetTotals(),
                damage,
                defender.ProcessAttackDamage(damage)
            ));
        }
    }
}