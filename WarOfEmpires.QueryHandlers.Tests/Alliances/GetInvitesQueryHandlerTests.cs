using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Exceptions;
using System;
using System.Collections.Generic;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Alliances {
    [TestClass]
    public sealed class GetInvitesQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly Alliance _alliance;

        public GetInvitesQueryHandlerTests() {
            _alliance = Substitute.For<Alliance>();

            _alliance.Id.Returns(1);
            _alliance.Code.Returns("FS");
            _alliance.Name.Returns("Føroyskir Samgonga");

            var invites = new List<Invite>() {
                AddInvite(1, "test1@test.com", "Test display name 1", UserStatus.Active, "Message", false, new DateTime(2020, 2, 15)),
                AddInvite(2, "test2@test.com", "Test display name 2", UserStatus.Inactive, null, false, new DateTime(2020, 1, 2)),
                AddInvite(3, "test3@test.com", "Test display name 3", UserStatus.Active, "Another message", true, new DateTime(2020, 1, 10))
            };

            _alliance.Invites.Returns(invites);

            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();

            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);
            player.User.Returns(user);
            player.Alliance.Returns(_alliance);
            _alliance.Members.Returns(new List<Player>() { player });

            _context.Alliances.Add(_alliance);
            _context.Users.Add(user);
            _context.Players.Add(player);
        }

        public Invite AddInvite(int id, string email, string displayName, UserStatus status, string message, bool isRead, DateTime date) {
            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();
            var invite = Substitute.For<Invite>();

            user.Email.Returns(email);
            user.Status.Returns(status);
            player.User.Returns(user);
            player.DisplayName.Returns(displayName);

            invite.Alliance.Returns(_alliance);
            invite.Message.Returns(message);
            invite.IsRead.Returns(isRead);
            invite.Date.Returns(date);
            invite.Player.Returns(player);

            _context.Users.Add(user);
            _context.Players.Add(player);

            return invite;
        }

        [TestMethod]
        public void GetInvitesQueryHandler_Returns_Correct_Information() {
            var handler = new GetInvitesQueryHandler(_context);
            var query = new GetInvitesQuery("test@test.com");

            var result = handler.Execute(query);

            result.Name.Should().Be("Føroyskir Samgonga");
            result.Invites.Should().HaveCount(2);
            result.Invites[0].PlayerName.Should().Be("Test display name 3");
            result.Invites[0].IsRead.Should().BeTrue();
            result.Invites[0].Date.Should().Be(new DateTime(2020, 2, 15));
            result.Invites[0].Message.Should().Be("Another message");
            result.Invites[1].PlayerName.Should().Be("Test display name 1");
            result.Invites[1].IsRead.Should().BeFalse();
            result.Invites[1].Date.Should().Be(new DateTime(2020, 1, 10));
            result.Invites[1].Message.Should().Be("Message");
        }
    }
}