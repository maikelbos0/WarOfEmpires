using System;
using System.Text.RegularExpressions;
using VDT.Core.DependencyInjection;

namespace WarOfEmpires.Utilities.Formatting {
    [TransientServiceImplementation(typeof(IEnumFormatter))]
    public class EnumFormatter : IEnumFormatter {
        private static readonly Regex _wordBoundaryFinder = new Regex("(\\B[A-Z])", RegexOptions.Compiled);

        public string ToString<TEnum>(TEnum value, bool capitalize = true) where TEnum : Enum {
            var s = _wordBoundaryFinder.Replace(value.ToString(), g => $" {g.Value.ToLower()}");

            if (capitalize) {
                return $"{char.ToUpper(s[0])}{s.Substring(1)}";
            }
            else {
                return s.ToLower();
            }
        }
    }
}