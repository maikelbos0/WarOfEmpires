﻿using System;
using System.Collections.Generic;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Attacks {
    public abstract class Attack : Entity {
        public const int AttackerMinimumStamina = 70;
        public const int DefenderMinimumStamina = 30;
        public const int StaminaRandomModifier = 10;
        public const decimal SurrenderResourcesPerTurn = 0.025m;
        public const decimal WonResourcesPerTurn = 0.05m;

        public virtual DateTime Date { get; protected set; }
        public virtual bool IsRead { get; set; }
        public virtual Player Attacker { get; protected set; }
        public virtual Player Defender { get; protected set; }
        public virtual AttackResult Result { get; protected set; } = AttackResult.Undefined;
        public virtual int Turns { get; protected set; }
        public virtual Resources Resources { get; protected set; } = new Resources();
        public virtual AttackType Type { get; protected set; }
        public virtual ICollection<AttackRound> Rounds { get; protected set; } = new List<AttackRound>();

        protected Attack() { }

        public Attack(Player attacker, Player defender, int turns) {
            if (turns < 1 || turns > 10) throw new ArgumentOutOfRangeException("turns", "Turns must be between 1 and 10");

            Date = DateTime.UtcNow;
            IsRead = false;
            Attacker = attacker;
            Defender = defender;
            Turns = turns;
        }

        public void Execute() {
            if (Result != AttackResult.Undefined) throw new InvalidOperationException("An attack can not be executed more than once");

            var attackerStamina = Attacker.Stamina;
            var defenderStamina = Defender.Stamina;

            if (attackerStamina < AttackerMinimumStamina) {
                Result = AttackResult.Fatigued;
            }
            else if (defenderStamina < DefenderMinimumStamina) {
                Result = AttackResult.Surrendered;
                Resources = GetBaseResources() * Turns * SurrenderResourcesPerTurn;
            }
            else {
                var random = new Random();
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
                    Resources = GetBaseResources() * Turns * WonResourcesPerTurn;                    
                }
                else {
                    Result = AttackResult.Defended;
                }
            }

            Attacker.ProcessAttack(Defender, Resources, Turns);
        }

        public void AddRound(int stamina, TroopType troopType, bool isAggressor, TroopInfo attackerTroopInfo, Player defender) {
            var damage = CalculateDamage(stamina, isAggressor, attackerTroopInfo, defender);

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

        public abstract Resources GetBaseResources();
        public abstract long CalculateDamage(int stamina, bool isAggressor, TroopInfo attackerTroopInfo, Player defender);
    }
}