using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class ProcessTurnCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly RankService _rankService = new RankService();

        public ProcessTurnCommandHandlerTests() {
            _repository = new PlayerRepository(_context);
        }

        [TestMethod]
        public void ProcessTurnCommandHandler_Calls_ProcessTurn_For_All_Active_Players() {
            var handler = new ProcessTurnCommandHandler(_repository, _rankService);
            var command = new ProcessTurnCommand();
            var rank = 3;

            for (var i = 0; i < 3; i++) {
                var user = Substitute.For<User>();
                var player = Substitute.For<Player>();

                user.Status.Returns(UserStatus.Active);
                player.User.Returns(user);
                player.Workers.Returns(new List<Workers>() { new Workers(WorkerType.Farmers, 7 + i) });

                _context.Users.Add(user);
                _context.Players.Add(player);
            }

            handler.Execute(command);

            foreach (var player in _context.Players) {
                player.Received().ProcessTurn(rank--);
            }

            _context.CallsToSaveChanges.Should().Be(1);
        }
    }
}