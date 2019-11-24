﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class GatherResourcesCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;

        public GatherResourcesCommandHandlerTests() {
            _repository = new PlayerRepository(_context);
        }

        [TestMethod]
        public void GatherResourcesCommandHandler_Calls_GatherResources_For_All_Active_Players() {
            var handler = new GatherResourcesCommandHandler(_repository);
            var command = new GatherResourcesCommand();

            for (var i = 0; i < 3; i++) {
                var user = Substitute.For<User>();
                var player = Substitute.For<Player>();

                user.Status.Returns(UserStatus.Active);
                player.User.Returns(user);

                _context.Users.Add(user);
                _context.Players.Add(player);
            }

            handler.Execute(command);

            foreach (var player in _context.Players) {
                player.Received().GatherResources();
            }
        }
    }
}