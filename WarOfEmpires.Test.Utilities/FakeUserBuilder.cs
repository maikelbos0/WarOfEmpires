using NSubstitute;
using System;
using WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Test.Utilities {
    public class FakeUserBuilder : FakeBuilder {
        public User User { get; }

        internal FakeUserBuilder(FakeWarContext context, int id, string email, string password, DateTime? lastOnline, UserStatus status, DateTime creationDate) : base(context) {
            User = Substitute.For<User>();
            User.Id.Returns(id);
            User.Email.Returns(email ?? $"test{id}@test.com");
            User.LastOnline.Returns(lastOnline);
            User.Status.Returns(status);
            User.Password.Returns(new Password(password));
            User.CreationDate.Returns(creationDate);
            Context.Users.Add(User);
        }
    }
}