using System;
using System.Collections.Generic;
using WarOfEmpires.Models.Empires;

namespace WarOfEmpires.Models.Attacks {
    public sealed class AttackDetailsViewModel : EntityViewModel {
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
        public string Type { get; set; }
        public int AttackerId { get; set; }
        public string Attacker { get; set; }
        public int? AttackerAllianceId { get; set; }
        public string AttackerAllianceCode { get; set; }
        public string AttackerAllianceName { get; set; }
        public int DefenderId { get; set; }
        public string Defender { get; set; }
        public int? DefenderAllianceId { get; set; }
        public string DefenderAllianceCode { get; set; }
        public string DefenderAllianceName { get; set; }
        public int Turns { get; set; }
        public List<AttackRoundDetailsViewModel> Rounds { get; set; } = new List<AttackRoundDetailsViewModel>();
        public string Result { get; set; }
        public ResourcesViewModel Resources { get; set; }
    }
}