namespace WarOfEmpires.Models.Attacks {
    public sealed class AttackRoundDetailsViewModel {
        public bool IsAggressor { get; set; }
        public string Attacker { get; set; }
        public string Defender { get; set; }
        public string TroopType { get; set; }
        public int Troops { get; set; }
        public long Damage { get; set; }
        public int ArcherSoldierCasualties { get; set; }
        public int ArcherMercenaryCasualties { get; set; }
        public int CavalrySoldierCasualties { get; set; }
        public int CavalryMercenaryCasualties { get; set; }
        public int FootmanSoldierCasualties { get; set; }
        public int FootmanMercenaryCasualties { get; set; }
    }
}