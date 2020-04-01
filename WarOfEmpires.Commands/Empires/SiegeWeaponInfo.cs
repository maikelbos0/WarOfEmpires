namespace WarOfEmpires.Commands.Empires {
    public class SiegeWeaponInfo {
        public string Type { get; }
        public string Count { get; }

        public SiegeWeaponInfo(string type, string count) {
            Type = type;
            Count = count;
        }
    }
}