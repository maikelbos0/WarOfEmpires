using System.Collections.Generic;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;

namespace WarOfEmpires.Domain.Siege {
    public static class SiegeWeaponDefinitionFactory {
        private static readonly Dictionary<SiegeWeaponType, SiegeWeaponDefinition> _siegeWeapons = new Dictionary<SiegeWeaponType, SiegeWeaponDefinition>();
        private static readonly Dictionary<TroopType, SiegeWeaponDefinition> _troopSiegeWeapons = new Dictionary<TroopType, SiegeWeaponDefinition>();

        static SiegeWeaponDefinitionFactory() {
            foreach (var definition in new[] { GenerateFireArrows(), GenerateBatteringRams(), GenerateScalingLadders() }) {
                _siegeWeapons.Add(definition.Type, definition);
                _troopSiegeWeapons.Add(definition.TroopType, definition);
            }
        }

        private static SiegeWeaponDefinition GenerateFireArrows() {
            return new SiegeWeaponDefinition(
                SiegeWeaponType.FireArrows,
                new Resources(wood: 80, ore: 40),
                TroopType.Archers,
                36,
                0.9,
                18,
                "Archers can fire flaming arrows to cause havoc behind enemy walls; it is however rare to recover any from the battle site"
            );
        }

        private static SiegeWeaponDefinition GenerateBatteringRams() {
            return new SiegeWeaponDefinition(
                SiegeWeaponType.BatteringRams,
                new Resources(wood: 200, ore: 100),
                TroopType.Cavalry,
                8,
                0.1,
                4,
                "Cavalry can keep any defences' gates down with battering rams; they can normally be reused"
            );
        }

        private static SiegeWeaponDefinition GenerateScalingLadders() {
            return new SiegeWeaponDefinition(
                SiegeWeaponType.ScalingLadders,
                new Resources(wood: 90, ore: 45),
                TroopType.Footmen,
                12,
                0.3,
                6,
                "Footmen can use ladders to scale any defences; they can usually be recovered from the battlefield"
            );
        }

        public static SiegeWeaponDefinition Get(SiegeWeaponType type) {
            return _siegeWeapons[type];
        }

        public static SiegeWeaponDefinition Get(TroopType type) {
            return _troopSiegeWeapons[type];
        }
    }
}