using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Attacks {
    public abstract class Attack : Entity {
        public const int AttackerMinimumStamina = 70;
        public const int DefenderMinimumStamina = 30;
        public const int StaminaRandomModifier = 10;
        public const decimal SurrenderResourcesPerTurn = 0.025m;
        public const decimal WonResourcesPerTurn = 0.05m;
        public const decimal WarResourcesModifier = 2.0m;
        public const decimal MinimumResourceArmyModifier = 0.5m;

        public virtual DateTime Date { get; protected set; }
        public virtual bool IsRead { get; set; }
        public virtual Player Attacker { get; protected set; }
        public virtual Player Defender { get; protected set; }
        public virtual AttackResult Result { get; protected set; } = AttackResult.Undefined;
        public virtual int Turns { get; protected set; }
        public virtual Resources Resources { get; protected set; } = new Resources();
        public virtual AttackType Type { get; protected set; }
        public virtual ICollection<AttackRound> Rounds { get; protected set; } = new List<AttackRound>();
        public virtual bool IsAtWar { get; protected set; }

        protected Attack() { }

        public Attack(Player attacker, Player defender, int turns) {
            if (turns < 1 || turns > 10) throw new ArgumentOutOfRangeException("turns", "Turns must be between 1 and 10");

            Date = DateTime.UtcNow;
            IsRead = false;
            Attacker = attacker;
            Defender = defender;
            Turns = turns;

            if (attacker.Alliance != null && defender.Alliance != null) {
                IsAtWar = attacker.Alliance.Wars.Any(w => w.Alliances.Contains(defender.Alliance));
            }
        }

        public void Execute() {
            if (Result != AttackResult.Undefined) throw new InvalidOperationException("An attack can't be executed more than once");

            var attackerStamina = Attacker.Stamina;
            var defenderStamina = Defender.Stamina;

            if (attackerStamina < AttackerMinimumStamina || Attacker.Troops.Sum(t => t.GetTotals()) == 0) {
                Result = AttackResult.Fatigued;
            }
            else if (IsSurrender()) {
                Result = AttackResult.Surrendered;
                Resources = GetResources(SurrenderResourcesPerTurn * GetArmyStrengthModifier(MinimumResourceArmyModifier));
            }
            else {
                // Measure army strength before the fighting happens
                var armyStrengthModifier = GetArmyStrengthModifier(MinimumResourceArmyModifier);
                var random = new Random();
                var calculatedAttackerStamina = random.Next(attackerStamina - StaminaRandomModifier, attackerStamina + StaminaRandomModifier);
                var calculatedDefenderStamina = random.Next(defenderStamina - StaminaRandomModifier, defenderStamina + StaminaRandomModifier);

                foreach (TroopType troopType in Enum.GetValues(typeof(TroopType))) {
                    AddRound(calculatedAttackerStamina, troopType, true, Attacker.GetTroopInfo(troopType), Defender);
                    AddRound(calculatedDefenderStamina, troopType, false, Defender.GetTroopInfo(troopType), Attacker);
                }

                if (Attacker.Stamina - attackerStamina > Defender.Stamina - defenderStamina) {
                    Result = AttackResult.Won;
                    Resources = GetResources(WonResourcesPerTurn * armyStrengthModifier);
                }
                else {
                    Result = AttackResult.Defended;
                }
            }
        }

        private Resources GetResources(decimal modifier) {
            if (IsAtWar) {
                modifier *= WarResourcesModifier;
            }

            var resources = GetBaseResources() * Turns * modifier;

            return resources - resources.SubtractSafe(Defender.Resources);
        }

        public void AddRound(int stamina, TroopType troopType, bool isAggressor, TroopInfo attackerTroopInfo, Player defender) {
            var damage = CalculateDamage(stamina, isAggressor, attackerTroopInfo, defender);

            if (damage == 0) {
                return;
            }

            Rounds.Add(new AttackRound(
                troopType,
                isAggressor,
                attackerTroopInfo.Troops.GetTotals(),
                damage,
                defender.ProcessAttackDamage(damage, IsAtWar)
            ));
        }

        public decimal GetArmyStrengthModifier(decimal minimum = 0) {
            var defenderTroops = Defender.Troops.Sum(t => t.GetTotals());
            var attackerTroops = Attacker.Troops.Sum(t => t.GetTotals());

            return Math.Max(minimum, 1.0m * defenderTroops / attackerTroops);
        }

        public abstract Resources GetBaseResources();
        public abstract long CalculateDamage(int stamina, bool isAggressor, TroopInfo attackerTroopInfo, Player defender);
        public abstract bool IsSurrender();
    }
}