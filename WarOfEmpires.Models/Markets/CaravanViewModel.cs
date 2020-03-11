using System;

namespace WarOfEmpires.Models.Markets {
    public sealed class CaravanViewModel {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Food { get; set; }
        public int Wood { get; set; }
        public int Stone { get; set; }
        public int Ore { get; set; }
    }
}