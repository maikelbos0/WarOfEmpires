namespace WarOfEmpires.Commands.Empires {
    public sealed class SiegeWeaponInfo {
        public string Type { get; }
        public int? Count { get; }

        public SiegeWeaponInfo(string type, int? count) {
            Type = type;
            Count = count;
        }
    }
}