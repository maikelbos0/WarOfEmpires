using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
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
    public sealed class GetReceivedInvitesQueryHandlerTest {
        private readonly FakeWarContext _context = new FakeWarContext();

        public GetReceivedInvitesQueryHandlerTest() {
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
                invite.Alliance.Returns(alliance);
                invite.Player.Returns(player);
                invite.Subject.Returns($"Invite from {allianceName}");
                invites.Add(invite);

                _context.Alliances.Add(alliance);
            }
        }

        [TestMethod]
        public void GetReceivedInvitesQueryHandler_Returns_Correct_Information() {
            var handler = new GetReceivedInvitesQueryHandler(_context);
            var query = new GetReceivedInvitesQuery("test@test.com");

            var result = handler.Execute(query).ToList();

            result.Should().HaveCount(2);
            result[0].Id.Should().Be(2);
            result[0].AllianceId.Should().Be(4);
            result[0].AllianceCode.Should().Be("ANOT");
            result[0].AllianceName.Should().Be("Another");
            result[0].Date.Should().Be(new DateTime(2020, 1, 29));
            result[0].Subject.Should().Be("Invite from Another");
            result[1].Id.Should().Be(1);
            result[1].AllianceId.Should().Be(3);
            result[1].AllianceCode.Should().Be("ALLI");
            result[1].AllianceName.Should().Be("Allies");
            result[1].Date.Should().Be(new DateTime(2020, 1, 30));
            result[1].Subject.Should().Be("Invite from Allies");
        }
    }
}