namespace WarOfEmpires.Client.Services;

public interface IPasswordStrengthCalculator {
    PasswordStrength Calculate(string? password);
}