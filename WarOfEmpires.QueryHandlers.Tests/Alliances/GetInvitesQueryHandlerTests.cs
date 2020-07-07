using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
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

            var invites = new List<Invite>() {
                AddInvite(1, 4, "test1@test.com", "Test display name 1", UserStatus.Active, "Message", false, new DateTime(2020, 2, 15)),
                AddInvite(2, 5, "test2@test.com", "Test display name 2", UserStatus.Inactive, null, false, new DateTime(2020, 1, 2)),
                AddInvite(3, 6, "test3@test.com", "Test display name 3", UserStatus.Active, "Another message", true, new DateTime(2020, 1, 10))
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

        public Invite AddInvite(int id, int playerId, string email, string displayName, UserStatus status, string subject, bool isRead, DateTime date) {
            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();
            var invite = Substitute.For<Invite>();

            user.Id.Returns(playerId);
            user.Email.Returns(email);
            user.Status.Returns(status);
            player.Id.Returns(playerId);
            player.User.Returns(user);
            player.DisplayName.Returns(displayName);

            invite.Id.Returns(id);
            invite.Alliance.Returns(_alliance);
            invite.Subject.Returns(subject);
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

            var result = handler.Execute(query).ToList();

            result.Should().HaveCount(2);
            result[0].Id.Should().Be(1);
            result[0].PlayerId.Should().Be(4);
            result[0].PlayerName.Should().Be("Test display name 1");
            result[0].IsRead.Should().BeFalse();
            result[0].Date.Should().Be(new DateTime(2020, 2, 15));
            result[0].Subject.Should().Be("Message");
            result[1].Id.Should().Be(3);
            result[1].PlayerId.Should().Be(6);
            result[1].PlayerName.Should().Be("Test display name 3");
            result[1].IsRead.Should().BeTrue();
            result[1].Date.Should().Be(new DateTime(2020, 1, 10));
            result[1].Subject.Should().Be("Another message");
        }
    }
}