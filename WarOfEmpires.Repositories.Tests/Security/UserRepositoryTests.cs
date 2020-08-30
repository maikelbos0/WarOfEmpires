using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Security;
using WarOfEmpires.Test.Utilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WarOfEmpires.Repositories.Tests.Security {
    [TestClass]
    public sealed class UserRepositoryTests {
        [TestMethod]
        public void UserRepository_TryGetByEmail_Succeeds() {
            var builder = new FakeBuilder()
                .WithUser(1);

            var repository = new UserRepository(builder.Context);

            var user = repository.TryGetByEmail("test1@test.com");

            user.Should().NotBeNull();
            user.Email.Should().Be("test1@test.com");
        }

        [TestMethod]
        public void UserRepository_TryGetByEmail_Returns_Null_For_Nonexistent_Email() {
            var builder = new FakeBuilder()
                .WithUser(1);

            var repository = new UserRepository(builder.Context);

            var user = repository.TryGetByEmail("nobody@test.com");

            user.Should().BeNull();
        }

        [TestMethod]
        public void UserRepository_TryGetByEmail_Does_Not_Save() {
            var builder = new FakeBuilder()
                .WithUser(1);

            var repository = new UserRepository(builder.Context);

            repository.TryGetByEmail("test@test.com");

            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void UserRepository_GetActiveByEmail_Succeeds() {
            var builder = new FakeBuilder()
                .WithUser(1);

            var repository = new UserRepository(builder.Context);

            var user = repository.GetActiveByEmail("test1@test.com");

            user.Should().NotBeNull();
            user.Email.Should().Be("test1@test.com");
        }

        [TestMethod]
        public void UserRepository_GetActiveByEmail_Throws_Exception_For_Nonexistent_Email() {
            var builder = new FakeBuilder()
                .WithUser(1);

            var repository = new UserRepository(builder.Context);

            Action action = () => repository.GetActiveByEmail("nobody@test.com");

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void UserRepository_GetActiveByEmail_Throws_Exception_For_Wrong_Status() {
            var builder = new FakeBuilder()
                .WithUser(1, email: "inactive@test.com", status: UserStatus.Inactive);

            var repository = new UserRepository(builder.Context);

            Action action = () => repository.GetActiveByEmail("inactive@test.com");

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void UserRepository_GetActiveByEmail_Does_Not_Save() {
            var builder = new FakeBuilder()
                .WithUser(1);

            var repository = new UserRepository(builder.Context);

            repository.GetActiveByEmail("test1@test.com");

            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void UserRepository_GetByEmail_Succeeds() {
            var builder = new FakeBuilder()
                .WithUser(1);

            var repository = new UserRepository(builder.Context);

            var user = repository.GetByEmail("test1@test.com", UserStatus.Active);

            user.Should().NotBeNull();
            user.Email.Should().Be("test1@test.com");
        }

        [TestMethod]
        public void UserRepository_GetByEmail_Throws_Exception_For_Nonexistent_Email() {
            var builder = new FakeBuilder()
                .WithUser(1);

            var repository = new UserRepository(builder.Context);

            Action action = () => repository.GetByEmail("nobody@test.com", UserStatus.Active);

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void UserRepository_GetByEmail_Throws_Exception_For_Wrong_Status() {
            var builder = new FakeBuilder()
                .WithUser(1, email: "inactive@test.com", status: UserStatus.Inactive);

            var repository = new UserRepository(builder.Context);

            Action action = () => repository.GetByEmail("inactive@test.com", UserStatus.Active);

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void UserRepository_GetByEmail_Does_Not_Save() {
            var builder = new FakeBuilder()
                .WithUser(1);

            var repository = new UserRepository(builder.Context);

            repository.GetByEmail("test1@test.com", UserStatus.Active);

            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void UserRepository_Add_Succeeds() {
            var context = new FakeWarContext();

            var repository = new UserRepository(context);
            var user = new User("new@test.com", "test");

            repository.Add(user);

            context.Users.Should().Contain(user);
        }

        [TestMethod]
        public void UserRepository_Add_Saves() {
            var context = new FakeWarContext();

            var repository = new UserRepository(context);

            repository.Add(new User("new@test.com", "test"));

            context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void UserRepository_Update_Saves() {
            var context = new FakeWarContext();

            var repository = new UserRepository(context);

            repository.Update();

            context.CallsToSaveChanges.Should().Be(1);
        }
    }
}