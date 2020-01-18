﻿namespace WarOfEmpires.Models.Empires {
    public class SiegeWeaponModel {
        public string Name { get; set; }
        public string TroopType { get; set; }
        public ResourcesViewModel Cost { get; set; }
        public int TroopCount { get; }
        public string Description { get; set; }
        public int CurrentCount { get; set; }
        public int CurrentMaintenance { get; set; }
        public int Count { get; set; }
    }
}