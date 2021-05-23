namespace WarOfEmpires.Extensions {
    public static class StringExtensions {
        public static string ToCamelCase(this string value) {
            if (!string.IsNullOrEmpty(value) && value.Length > 0) {
                return char.ToLowerInvariant(value[0]) + value.Substring(1);
            }

            return value;
        }

        public static string ToPascalCase(this string value) {
            if (!string.IsNullOrEmpty(value) && value.Length > 0) {
                return char.ToUpperInvariant(value[0]) + value.Substring(1);
            }

            return value;
        }
    }
}
