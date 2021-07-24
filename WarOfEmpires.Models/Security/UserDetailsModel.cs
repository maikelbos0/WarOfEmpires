using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Security {
    public sealed class UserDetailsModel : EntityViewModel {
        [DisplayName("Email address")]
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "This email address is invalid")]
        public string Email { get; set; }

        [DisplayName("Display name")]
        [Required(ErrorMessage = "Display name is required")]
        [MaxLength(25, ErrorMessage = "Display name can only be 25 characters long")]
        public string DisplayName { get; set; }
        
        [DisplayName("Alliance code")]
        [Required(ErrorMessage = "Alliance code is required")]
        [MaxLength(4, ErrorMessage = "Code must be 4 characters or less")]
        public string AllianceCode { get; set; }
        
        [DisplayName("Alliance name")]
        [Required(ErrorMessage = "Alliance name is required")]
        public string AllianceName { get; set; }
        
        [DisplayName("Status")]
        [Required(ErrorMessage = "Status is required")]        
        public string Status { get; set; }

        [DisplayName("Is admin")]
        public bool IsAdmin { get; set; }
        
        public DateTime? LastOnline { get; set; }
    }
}
