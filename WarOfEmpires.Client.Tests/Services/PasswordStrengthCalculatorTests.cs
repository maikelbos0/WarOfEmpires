using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Client.Services;

namespace WarOfEmpires.Client.Tests.Services;

[TestClass]
public sealed class PasswordStrengthCalculatorTests {
    [DataTestMethod]
    [DataRow(null, PasswordStrength.None)]
    [DataRow("", PasswordStrength.None)]
    [DataRow("a", PasswordStrength.Weakest)]
    [DataRow("abcdefghij", PasswordStrength.Weakest)]
    [DataRow("aA!", PasswordStrength.Weakest)]
    [DataRow("aBcDeFgHiJ", PasswordStrength.Weak)]
    [DataRow("aBcDeFgHi!", PasswordStrength.Medium)]
    [DataRow("aBcDeFgH1!", PasswordStrength.Strong)]
    [DataRow("aB1! aaaaa", PasswordStrength.Strongest)]
    public void PasswordStrengthCalculator_Calculate_Succeeds(string password, PasswordStrength expectedStrength) {
        var calculator = new PasswordStrengthCalculator();

        calculator.Calculate(password).Should().Be(expectedStrength);
    }
}
