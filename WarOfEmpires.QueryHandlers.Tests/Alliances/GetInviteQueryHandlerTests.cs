using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
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
    public sealed class GetInviteQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly Alliance _alliance;

        public GetInviteQueryHandlerTests() {
            _alliance = Substitute.For<Alliance>();

            var invites = new List<Invite>() {
                AddInvite(1, 4, "test1@test.com", "Test display name 1", UserStatus.Active, "Message", "Message body", false, new DateTime(2020, 2, 15)),
                AddInvite(3, 6, "test3@test.com", "Test display name 3", UserStatus.Active, "Another message", "Another body", true, new DateTime(2020, 1, 10))
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

            var wrongAlliance = Substitute.For<Alliance>();
            var wrongUser = Substitute.For<User>();
            var wrongPlayer = Substitute.For<Player>();

            wrongUser.Email.Returns("wrong@test.com");
            wrongUser.Status.Returns(UserStatus.Active);
            wrongPlayer.User.Returns(wrongUser);
            wrongPlayer.Alliance.Returns(wrongAlliance);
            wrongAlliance.Members.Returns(new List<Player>() { wrongPlayer });
            wrongAlliance.Invites.Returns(new List<Invite>());

            _context.Alliances.Add(wrongAlliance);
            _context.Users.Add(wrongUser);
            _context.Players.Add(wrongPlayer);
        }

        public Invite AddInvite(int id, int playerId, string email, string displayName, UserStatus status, string subject, string body, bool isRead, DateTime date) {
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
            invite.Body.Returns(body);
            invite.IsRead.Returns(isRead);
            invite.Date.Returns(date);
            invite.Player.Returns(player);

            _context.Users.Add(user);
            _context.Players.Add(player);

            return invite;
        }

        [TestMethod]
        public void GetInviteQueryHandler_Returns_Correct_Information() {
            var handler = new GetInviteQueryHandler(_context);
            var query = new GetInviteQuery("test@test.com", "1");

            var result = handler.Execute(query);

            result.Id.Should().Be(1);
            result.PlayerId.Should().Be(4);
            result.PlayerName.Should().Be("Test display name 1");
            result.IsRead.Should().BeFalse();
            result.Date.Should().Be(new DateTime(2020, 2, 15));
            result.Subject.Should().Be("Message");
            result.Body.Should().Be("Message body");
        }

        [TestMethod]
        public void GetInviteQueryHandler_Throws_Exception_For_Message_Sent_By_Different_Alliance() {
            var handler = new GetInviteQueryHandler(_context);
            var query = new GetInviteQuery("wrong@test.com", "1");

            Action action = () => handler.Execute(query);

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void GetInviteQueryHandler_Throws_Exception_For_Alphanumeric_MessageId() {
            var handler = new GetInviteQueryHandler(_context);
            var query = new GetInviteQuery("test@test.com", "A");

            Action action = () => handler.Execute(query);

            action.Should().Throw<FormatException>();
        }
    }
}