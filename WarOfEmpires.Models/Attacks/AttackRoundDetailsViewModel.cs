namespace WarOfEmpires.Models.Attacks {
    public sealed class AttackRoundDetailsViewModel {
        public bool IsAggressor { get; set; }
        public string Attacker { get; set; }
        public string Defender { get; set; }
        public string TroopType { get; set; }
        public int Troops { get; set; }
        public long Damage { get; set; }
        public int SoldierCasualties { get; set; }
        public int MercenaryCasualties { get; set; }
    }
}