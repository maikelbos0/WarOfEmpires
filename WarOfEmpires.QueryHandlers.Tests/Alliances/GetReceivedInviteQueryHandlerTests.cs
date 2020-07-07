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
    public sealed class GetReceivedInviteQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        public GetReceivedInviteQueryHandlerTests() {
            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();
            var invites = new List<Invite>();

            user.Id.Returns(1);
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);
            player.Id.Returns(1);
            player.User.Returns(user);
            player.DisplayName.Returns("Test");
            player.Invites.Returns(invites);

            _context.Users.Add(user);
            _context.Players.Add(player);

            foreach (var allianceName in new[] { "Allies", "Another" }) {
                var alliance = Substitute.For<Alliance>();
                var invite = Substitute.For<Invite>();

                alliance.Id.Returns(invites.Count + 3);
                alliance.Code.Returns(allianceName.ToUpper().Substring(0, 4));
                alliance.Name.Returns(allianceName);

                invite.Id.Returns(invites.Count + 1);
                invite.Date.Returns(new DateTime(2020, 1, 30 - invites.Count));
                invite.IsRead.Returns(true);
                invite.Alliance.Returns(alliance);
                invite.Player.Returns(player);
                invite.Subject.Returns($"Invite from {allianceName}");
                invite.Body.Returns($"Hey, here's your invite into {allianceName}");
                invites.Add(invite);

                _context.Alliances.Add(alliance);
            }

            var wrongUser = Substitute.For<User>();
            var wrongPlayer = Substitute.For<Player>();

            wrongUser.Email.Returns("wrong@test.com");
            wrongUser.Status.Returns(UserStatus.Active);
            wrongPlayer.User.Returns(wrongUser);
            wrongPlayer.Invites.Returns(new List<Invite>() { });

            _context.Users.Add(wrongUser);
            _context.Players.Add(wrongPlayer);
        }

        [TestMethod]
        public void GetReceivedInviteQueryHandler_Returns_Correct_Information() {
            var handler = new GetReceivedInviteQueryHandler(_context);
            var query = new GetReceivedInviteQuery("test@test.com", "2");

            var result = handler.Execute(query);

            result.Id.Should().Be(2);
            result.AllianceId.Should().Be(4);
            result.AllianceCode.Should().Be("ANOT");
            result.AllianceName.Should().Be("Another");
            result.IsRead.Should().BeTrue();
            result.Date.Should().Be(new DateTime(2020, 1, 29));
            result.Subject.Should().Be("Invite from Another");
            result.Body.Should().Be("Hey, here's your invite into Another");
        }

        [TestMethod]
        public void GetReceivedInviteQueryHandler_Throws_Exception_For_Message_Received_By_Different_Player() {
            var handler = new GetReceivedInviteQueryHandler(_context);
            var query = new GetReceivedInviteQuery("wrong@test.com", "2");

            Action action = () => handler.Execute(query);

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void GetReceivedInviteQueryHandler_Throws_Exception_For_Alphanumeric_MessageId() {
            var handler = new GetReceivedInviteQueryHandler(_context);
            var query = new GetReceivedInviteQuery("test@test.com", "A");

            Action action = () => handler.Execute(query);

            action.Should().Throw<FormatException>();
        }
    }
}