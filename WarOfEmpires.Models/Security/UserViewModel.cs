using System;

namespace WarOfEmpires.Models.Security {
    public sealed class UserViewModel : ViewModel {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public string StartDateString { get { return StartDate?.ToShortDateString(); } }
    }
}