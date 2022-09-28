using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using WarOfEmpires.DataAnnotations;

namespace WarOfEmpires.CompositeModels {
    public sealed class AvatarCompositeModel<TBase> {
        [DisplayName("Upload new avatar")]
        [MaxFileSize(1024 * 1024, ErrorMessage = "New avatar size has to be 1 megabyte or smaller")]
        [FileExtension(".jpg", ".jpeg", ".png", ErrorMessage = "New avatar has to be a jpeg or png image")]
        public IFormFile Avatar { get; set; }

        public TBase Base { get; set; }
    }
}
