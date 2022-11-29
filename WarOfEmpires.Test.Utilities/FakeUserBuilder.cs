using NSubstitute;
using System;
using System.Collections.Generic;
using WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Test.Utilities {
    public class FakeUserBuilder : FakeBuilder {
        public User User { get; }

        internal FakeUserBuilder(FakeWarContext context, int id, string email, string password, DateTime? lastOnline, UserStatus status) : base(context) {
            User = Substitute.For<User>();
            User.Id.Returns(id);
            User.Email.Returns(email ?? $"test{id}@test.com");
            User.LastOnline.Returns(lastOnline);
            User.Status.Returns(status);
            User.Password.Returns(new Password(password));
            User.RefreshTokenFamilies.Returns(new List<RefreshTokenFamily>());

            Context.Users.Add(User);
        }

        public FakeUserBuilder WithRefreshTokenFamily(int id, out RefreshTokenFamily family, Guid requestId, byte[] token) {
            family = Substitute.For<RefreshTokenFamily>();

            family.Id.Returns(id);
            family.RequestId.Returns(requestId);
            family.CurrentToken.Returns(token);

            User.RefreshTokenFamilies.Add(family);

            return this;
        }

        public FakeUserBuilder WithRefreshTokenFamily(int id, Guid requestId, byte[] token) {
            return WithRefreshTokenFamily(id, out _, requestId, token);
        }
    }
}
