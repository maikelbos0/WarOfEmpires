using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Domain.Tests.Security {
    [TestClass]
    public class RefreshTokenFamilyTests {
        [TestMethod]
        public void RefreshTokenFamily_Constructor_Succeeds() {
            var requestId = Guid.NewGuid();

            var family = new RefreshTokenFamily(requestId, new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 });

            family.CurrentToken.Should().BeEquivalentTo(new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 });
            family.RequestId.Should().Be(requestId);
        }

        [TestMethod]
        public void RefreshTokenFamily_Rotate_Succeeds() {
            var requestId = Guid.NewGuid();
            var family = new RefreshTokenFamily(Guid.NewGuid(), new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 });

            family.PreviousRefreshTokens.Add(new PreviousRefreshToken(new byte[] { 2, 2, 2, 2, 2, 2, 2, 2 }));

            family.RotateRefreshToken(requestId, new byte[] { 7, 7, 7, 7, 7, 7, 7, 7 });

            family.CurrentToken.Should().BeEquivalentTo(new byte[] { 7, 7, 7, 7, 7, 7, 7, 7 });
            family.RequestId.Should().Be(requestId);
            family.PreviousRefreshTokens.Should().HaveCount(2);
            family.PreviousRefreshTokens.Should().ContainSingle(t => new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 }.SequenceEqual(t.Token));
            family.PreviousRefreshTokens.Should().ContainSingle(t => new byte[] { 2, 2, 2, 2, 2, 2, 2, 2 }.SequenceEqual(t.Token));
        }
    }
}
