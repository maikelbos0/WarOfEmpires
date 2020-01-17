using System.Collections.Generic;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;

namespace WarOfEmpires.Domain.Siege {
    public static class SiegeWeaponDefinitionFactory {
        private static readonly Dictionary<SiegeWeaponType, SiegeWeaponDefinition> _siegeWeapons = new Dictionary<SiegeWeaponType, SiegeWeaponDefinition>();

        static SiegeWeaponDefinitionFactory() {
            _siegeWeapons.Add(SiegeWeaponType.FireArrows, GenerateFireArrows());
            _siegeWeapons.Add(SiegeWeaponType.BatteringRams, GenerateBatteringRams());
            _siegeWeapons.Add(SiegeWeaponType.ScalingLadders, GenerateScalingLadders());
        }

        private static SiegeWeaponDefinition GenerateFireArrows() {
            return new SiegeWeaponDefinition(
                new Resources(800, 400),
                TroopType.Archers,
                36,
                0.9,
                "Archers can fire flaming arrows to cause havoc behind enemy walls; it is however rare to recover any from the battle site"
            );
        }

        private static SiegeWeaponDefinition GenerateBatteringRams() {
            return new SiegeWeaponDefinition(
                new Resources(wood: 2000, ore: 1000),
                TroopType.Cavalry,
                8,
                0.1,
                "Cavalry can keep any defences' gates down with battering rams; they can be recovered from the battle site afterwards unless they break"
            );
        }

        private static SiegeWeaponDefinition GenerateScalingLadders() {
            return new SiegeWeaponDefinition(
                new Resources(wood: 900, ore: 450),
                TroopType.Cavalry,
                12,
                0.3,
                "Footmen can use ladders to scale any defences; they can usually be recovered from the battlefield"
            );
        }

        public static SiegeWeaponDefinition Get(SiegeWeaponType type) {
            return _siegeWeapons[type];
        }
    }
}