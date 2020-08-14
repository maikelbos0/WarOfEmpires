using NSubstitute;
using System;
using WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Test.Utilities {
    public class FakeUserBuilder : FakeBuilder {
        public User User { get; }

        internal FakeUserBuilder(FakeWarContext context, int id, string email, DateTime? lastOnline, UserStatus status) : base(context) {
            User = Substitute.For<User>();
            User.Id.Returns(id);
            User.Email.Returns(email ?? $"test{id}@test.com");
            User.LastOnline.Returns(lastOnline);
            User.Status.Returns(status);
            Context.Users.Add(User);
        }
    }
}