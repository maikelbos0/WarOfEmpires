using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WarOfEmpires.Models.DataAnnotations;

namespace WarOfEmpires.Models.Players {
    public sealed class CreatePlayerModel {
        public List<RaceViewModel> Races { get; set; } = new List<RaceViewModel>();

        [DisplayName("Race")]
        [Required(ErrorMessage = "Race is required")]
        public string Race { get; set; }

        [DisplayName("Display name")]
        [Required(ErrorMessage = "Display name is required")]
        [MaxLength(25, ErrorMessage = "Display name can only be 25 characters long")]
        public string DisplayName { get; set; }

        [DisplayName("Full name")]
        [MaxLength(100, ErrorMessage = "Full name can only be 100 characters long")]
        public string FullName { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Upload avatar")]
        [MaxFileSize(1024 * 1024, ErrorMessage = "Avatar size has to be 1 megabyte or smaller")]
        [FileExtension(".jpg", ".jpeg", ".png", ErrorMessage = "Avatar has to be a jpeg or png image")]
        public IFormFile Avatar { get; set; }
    }
}
