using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Players {
    public sealed class ProfileModel {
        [DisplayName("Full name")]
        [MaxLength(100, ErrorMessage = "Full name can only be 100 characters long")]
        public string FullName { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Upload new avatar")]
        public IFormFile Avatar { get; set; }

        public string AvatarLocation { get; set; }
    }
}
