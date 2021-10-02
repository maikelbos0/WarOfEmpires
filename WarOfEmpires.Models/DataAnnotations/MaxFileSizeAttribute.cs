using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace WarOfEmpires.Models.DataAnnotations {
    public sealed class MaxFileSizeAttribute : ValidationAttribute {
        public int Size { get; }

        public MaxFileSizeAttribute(int size) {
            Size = size;
        }

        public override bool IsValid(object value) {
            return !(value is IFormFile file && file.Length > Size);
        }

        public override string FormatErrorMessage(string name) {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Size);
        }
    }
}
