using System;

namespace WarOfEmpires.Utilities.Formatting {
    public interface IEnumFormatter {
        string ToString<TEnum>(TEnum value, bool capitalize = true) where TEnum : Enum;
    }
}