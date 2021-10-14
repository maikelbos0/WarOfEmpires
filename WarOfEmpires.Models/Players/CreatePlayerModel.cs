using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WarOfEmpires.Models.DataAnnotations;

namespace WarOfEmpires.Models.Players {
    public sealed class CreatePlayerModel {
        // TODO add race here

        [DisplayName("Display name")]
        [Required(ErrorMessage = "Display name is required")]
        [MaxLength(25, ErrorMessage = "Display name can only be 25 characters long")]
        public string DisplayName { get; set; }

        [DisplayName("Full name")]
        [MaxLength(100, ErrorMessage = "Full name can only be 100 characters long")]
        public string FullName { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Upload new avatar")]
        [MaxFileSize(1024 * 1024, ErrorMessage = "New avatar size has to be 1 megabyte or smaller")]
        [FileExtension(".jpg", ".jpeg", ".png", ErrorMessage = "New avatar has to be a jpeg or png image")]
        public IFormFile Avatar { get; set; }
    }
}
