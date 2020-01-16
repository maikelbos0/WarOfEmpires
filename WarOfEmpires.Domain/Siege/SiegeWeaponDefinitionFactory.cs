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
                new Resources(),
                TroopType.Archers,
                1,
                new Resources(wood: 1, ore: 1),
                1
            );
        }

        private static SiegeWeaponDefinition GenerateBatteringRams() {
            return new SiegeWeaponDefinition(
                new Resources(gold: 20000, wood: 5000, ore: 1000),
                TroopType.Cavalry,
                8,
                new Resources(),
                8
            );
        }

        private static SiegeWeaponDefinition GenerateScalingLadders() {
            return new SiegeWeaponDefinition(
                new Resources(gold: 5000, wood: 1000, ore: 200),
                TroopType.Cavalry,
                12,
                new Resources(wood: 5, ore: 1),
                8
            );
        }

        public static SiegeWeaponDefinition Get(SiegeWeaponType type) {
            return _siegeWeapons[type];
        }
    }
}