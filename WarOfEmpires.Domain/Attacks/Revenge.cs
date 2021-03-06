﻿using System.Linq;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Attacks {
    public class Revenge : Attack {
        public const int DefenceDamageModifier = 5;
        
        protected Revenge() { }

        public Revenge(Player attacker, Player defender, int turns) : base(attacker, defender, turns) {
            Type = AttackType.Revenge;
        }

        public override long CalculateDamage(int stamina, bool isAggressor, TroopInfo attackerTroopInfo, Player defender) {
            var multiplier = 1.0;

            if (isAggressor) {
                // This multiplier makes sure damage goes down the higher the defence bonus of the defender
                // It's a limit function from 1 that approaches 0 as the bonus goes up
                multiplier = 1.0 / (defender.GetBuildingBonus(BuildingType.Defences) + DefenceDamageModifier) * DefenceDamageModifier;
            }

            return (int)(attackerTroopInfo.GetTotalAttack(multiplier) * Turns * stamina / 100.0);
        }

        public override Resources GetBaseResources() {
            return new Resources();
        }

        public override bool IsSurrender() {
            return Defender.Troops.Sum(t => t.GetTotals()) == 0;
        }
    }
}
