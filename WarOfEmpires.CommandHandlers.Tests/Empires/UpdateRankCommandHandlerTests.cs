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
    public sealed class UpdateRankCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly RankService _rankService = new RankService();

        public UpdateRankCommandHandlerTests() {
            _repository = new PlayerRepository(_context);
        }

        [TestMethod]
        public void UpdateRankCommandHandler_Updates_Rank_For_Active_Players() {
            var handler = new UpdateRankCommandHandler(_repository, _rankService);
            var command = new UpdateRankCommand();
            var rank = 3;

            foreach (var status in new[] { UserStatus.Active, UserStatus.Inactive, UserStatus.New }) {
                for (var i = 0; i < 3; i++) {
                    var user = Substitute.For<User>();
                    var player = Substitute.For<Player>();

                    user.Status.Returns(status);
                    player.User.Returns(user);
                    player.Workers.Returns(new List<Workers>() {
                       new Workers(WorkerType.WoodWorkers, 7 + i)
                    });

                    _context.Users.Add(user);
                    _context.Players.Add(player);
                }
            }

            handler.Execute(command);

            foreach (var player in _context.Players) {
                if (player.User.Status == UserStatus.Active) {
                    player.Rank.Should().Be(rank--);
                }
                else {
                    player.Rank.Should().Be(0);
                }
            }

            _context.CallsToSaveChanges.Should().Be(1);
        }
    }
}