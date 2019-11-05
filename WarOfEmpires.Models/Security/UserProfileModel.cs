using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Security {
    public sealed class UserProfileModel{
        [DisplayName("Display name")]
        [MaxLength(50, ErrorMessage = "Display name can only be 50 characters long")]
        public string DisplayName { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Make email public")]
        public bool ShowEmail { get; set; }
    }
}