using System;

namespace WarOfEmpires.Models.Players {
    public sealed class PlayerDetailsViewModel : EntityViewModel {
        public string Status { get; set; }
        public int Rank { get; set; }
        public string DisplayName { get; set; }
        public int? AllianceId { get; set; }
        public string AllianceCode { get; set; }
        public string AllianceName { get; set; }
        public int Population { get; set; }
        public string Title { get; set; }
        public string Defences { get; set; }
        public bool CanBeAttacked { get; set; }
        public TimeSpan? GrandOverlordTime { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string AvatarLocation { get; set; }
    }
}