using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;

namespace WarOfEmpires.Models.DataAnnotations {
    public sealed class FileExtensionAttribute : ValidationAttribute {
        public string[] Extensions { get; }

        public FileExtensionAttribute(params string[] extensions) {
            Extensions = extensions;
        }

        public override bool IsValid(object value) {
            return !(value is IFormFile file) || Extensions.Contains(Path.GetExtension(file.FileName));
        }

        public override string FormatErrorMessage(string name) {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, string.Join(", ", Extensions));
        }
    }
}
