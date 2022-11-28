using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Domain.Tests.Security {
    [TestClass]
    public class RefreshTokenFamilyTests {
        [TestMethod]
        public void RefreshTokenFamily_Rotate_Succeeds() {
            var family = new RefreshTokenFamily(new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 });
            family.ExpiredRefreshTokens.Add(new ExpiredRefreshToken(new byte[] { 2, 2, 2, 2, 2, 2, 2, 2 }));

            family.RotateRefreshToken(new byte[] { 7, 7, 7, 7, 7, 7, 7, 7 });

            family.CurrentToken.Should().BeEquivalentTo(new byte[] { 7, 7, 7, 7, 7, 7, 7, 7 });
            family.ExpiredRefreshTokens.Should().HaveCount(2);
            family.ExpiredRefreshTokens.Should().ContainSingle(t => new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 }.SequenceEqual(t.Token));
            family.ExpiredRefreshTokens.Should().ContainSingle(t => new byte[] { 2, 2, 2, 2, 2, 2, 2, 2 }.SequenceEqual(t.Token));
        }
    }
}
