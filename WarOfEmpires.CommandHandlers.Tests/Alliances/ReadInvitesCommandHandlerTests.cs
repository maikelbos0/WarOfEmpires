using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Alliances;
using WarOfEmpires.Commands.Alliances;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Alliances {
    [TestClass]
    public sealed class ReadInvitesCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Invite _invite;

        public ReadInvitesCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            _invite = Substitute.For<Invite>();

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);

            var player = Substitute.For<Player>();
            player.Invites.Returns(new List<Invite>() { _invite });
            player.User.Returns(user);

            _context.Users.Add(user);
            _context.Players.Add(player);
        }

        [TestMethod]
        public void ReadInvitesCommandHandler_Succeeds() {
            var handler = new ReadInvitesCommandHandler(_repository);
            var command = new ReadInvitesCommand("test@test.com");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _invite.Received().IsRead = true;
            _context.CallsToSaveChanges.Should().Be(1);
        }
    }
}