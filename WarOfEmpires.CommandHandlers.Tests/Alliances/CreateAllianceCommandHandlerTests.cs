using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq;
using WarOfEmpires.CommandHandlers.Alliances;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Alliances {
    [TestClass]
    public sealed class CreateAllianceCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _player;

        public CreateAllianceCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);
            _player = Substitute.For<Player>();
            _player.User.Returns(user);
            _context.Players.Add(_player);
        }


        [TestMethod]
        public void CreateAllianceCommandHandler_Succeeds() {
            var handler = new CreateAllianceCommandHandler(_repository);
            var command = new CreateAllianceCommand("test@test.com", "CODE", "The Alliance");

            var result = handler.Execute(command);
            var alliance = _context.Alliances.SingleOrDefault();

            result.Success.Should().BeTrue();
            alliance.Should().NotBeNull();
            alliance.Code.Should().Be("CODE");
            alliance.Name.Should().Be("The Alliance");
            alliance.Leader.Should().Be(_player);
            alliance.Members.Should().Contain(_player);
            _context.CallsToSaveChanges.Should().Be(2);
        }

        [TestMethod]
        public void CreateAllianceCommandHandler_Throws_Exception_For_Nonexistent_Player() {
            var handler = new CreateAllianceCommandHandler(_repository);
            var command = new CreateAllianceCommand("wrong@test.com", "CODE", "The Alliance");

            Action action = () => handler.Execute(command);

            action.Should().Throw<InvalidOperationException>();
            _context.Alliances.Should().BeEmpty();
            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void CreateAllianceCommandHandler_Fails_For_Too_Long_Code() {
            var handler = new CreateAllianceCommandHandler(_repository);
            var command = new CreateAllianceCommand("test@test.com", "CODE1", "The Alliance");

            var result = handler.Execute(command);

            result.Should().HaveError("Code", "Code must be 4 characters or less");
            _context.Alliances.Should().BeEmpty();
            _context.CallsToSaveChanges.Should().Be(0);
        }
    }
}