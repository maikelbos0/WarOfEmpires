using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace WarOfEmpires.Models.DataAnnotations {
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class MaxFileSizeAttribute : ValidationAttribute, IClientModelValidator {
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

        public void AddValidation(ClientModelValidationContext context) {
            context.Attributes["data-val"] = "true";
            context.Attributes["data-val-maxfilesize"] = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            context.Attributes["data-val-maxfilesize-size"] = Size.ToString();
        }
    }
}
