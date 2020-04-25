using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class UpdateRankCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly RankService _rankService = Substitute.For<RankService>();

        public UpdateRankCommandHandlerTests() {
            _repository = new PlayerRepository(_context);
        }

        [TestMethod]
        public void UpdateRankCommandHandler_Calls_RankService_Update_For_Active_Players() {
            var handler = new UpdateRankCommandHandler(_repository, _rankService);
            var command = new UpdateRankCommand();

            foreach (var status in new[] { UserStatus.Active, UserStatus.Inactive, UserStatus.New }) {
                var user = Substitute.For<User>();
                var player = Substitute.For<Player>();

                user.Status.Returns(status);
                player.User.Returns(user);

                _context.Users.Add(user);
                _context.Players.Add(player);
            }

            handler.Execute(command);
            _rankService.Received().Update(Arg.Is<List<Player>>(players => players.SequenceEqual(_repository.GetAll())));
            _context.CallsToSaveChanges.Should().Be(1);
        }
    }
}