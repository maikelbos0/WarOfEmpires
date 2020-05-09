﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Players {
    [TestClass]
    public sealed class GetCurrentPlayerQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly Alliance _alliance;
        private readonly Player _player;
        private readonly User _user;

        public GetCurrentPlayerQueryHandlerTests() {
            _alliance = Substitute.For<Alliance>();
            _player = Substitute.For<Player>();
            _user = Substitute.For<User>();

            _user.Email.Returns("test@test.com");
            _user.IsAdmin.Returns(false);
            _player.User.Returns(_user);
            _player.DisplayName.Returns("Test");
            _player.Alliance.Returns((Alliance)null);
            _context.Players.Add(_player);
        }

        [TestMethod]
        public void GetCurrentPlayerQueryHandler_Returns_Correct_Information() {
            var query = new GetCurrentPlayerQuery("test@test.com");
            var handler = new GetCurrentPlayerQueryHandler(_context);

            var result = handler.Execute(query);

            result.IsAuthenticated.Should().BeTrue();
            result.IsAdmin.Should().BeFalse();
            result.IsInAlliance.Should().BeFalse();
            result.DisplayName.Should().Be("Test");
        }

        [TestMethod]
        public void GetCurrentPlayerQueryHandler_Returns_Correct_Information_IsAdmin_IsInAlliance() {
            _user.IsAdmin.Returns(true);
            _player.Alliance.Returns(_alliance);

            var query = new GetCurrentPlayerQuery("test@test.com");
            var handler = new GetCurrentPlayerQueryHandler(_context);

            var result = handler.Execute(query);

            result.IsAdmin.Should().BeTrue();
            result.IsInAlliance.Should().BeTrue();
        }

        [TestMethod]
        public void GetCurrentPlayerQueryHandler_Throws_Exception_For_Nonexistent_Email() {
            var query = new GetCurrentPlayerQuery("wrong@test.com");
            var handler = new GetCurrentPlayerQueryHandler(_context);

            Action action = () => handler.Execute(query);

            action.Should().Throw<InvalidOperationException>();
        }
    }
}