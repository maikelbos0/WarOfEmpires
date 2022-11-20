using System;
using System.Linq;

namespace WarOfEmpires.Client.Services;

public sealed class PasswordStrengthCalculator : IPasswordStrengthCalculator {
    public PasswordStrength Calculate(string? password) {
        if (string.IsNullOrEmpty(password)) {
            return PasswordStrength.None;
        }

        var lengthStrength = Math.Min((password.Length) / 2, 5);
        var characterStrength = Math.Min(password.DistinctBy(c => char.GetUnicodeCategory(c)).Count(), 5);

        return (PasswordStrength)Math.Max(Math.Min(lengthStrength, characterStrength), 1);
    }
}
