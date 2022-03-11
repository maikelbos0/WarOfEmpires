using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;

namespace WarOfEmpires.Models.DataAnnotations {
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class FileExtensionAttribute : ValidationAttribute, IClientModelValidator {
        public string[] Extensions { get; }

        public FileExtensionAttribute(params string[] extensions) {
            Extensions = extensions;
        }

        public override bool IsValid(object value) {
            return value is not IFormFile file || Extensions.Any(e => string.Equals(e, Path.GetExtension(file.FileName), StringComparison.OrdinalIgnoreCase));
        }

        public override string FormatErrorMessage(string name) {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, string.Join(", ", Extensions));
        }

        public void AddValidation(ClientModelValidationContext context) {
            context.Attributes["data-val"] = "true";
            context.Attributes["data-val-fileextension"] = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            context.Attributes["data-val-fileextension-extensions"] = string.Join(",", Extensions);
        }
    }
}
