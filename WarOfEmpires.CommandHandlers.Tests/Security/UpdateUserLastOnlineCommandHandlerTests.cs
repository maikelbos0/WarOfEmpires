using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.CommandHandlers.Security;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Security;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Security {
    [TestClass]
    public sealed class UpdateUserLastOnlineCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly UserRepository _repository;
        private readonly User _user;

        public UpdateUserLastOnlineCommandHandlerTests() {
            _repository = new UserRepository(_context);

            _user = Substitute.For<User>();
            _user.Email.Returns("test@test.com");
            _user.Status.Returns(UserStatus.Active);

            _context.Users.Add(_user);
        }

        [TestMethod]
        public void UpdateUserLastOnlineCommandHandler_Succeeds() {
            var handler = new UpdateUserLastOnlineCommandHandler(_repository);
            var command = new UpdateUserLastOnlineCommand("test@test.com");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _user.Received().WasOnline();
            _context.CallsToSaveChanges.Should().Be(1);
        }
    }
}