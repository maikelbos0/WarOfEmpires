using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.CommandHandlers.Empires;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class UntrainTroopsCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;
        private readonly Player _player;

        public UntrainTroopsCommandHandlerTests() {
            _repository = new PlayerRepository(_context);

            var user = Substitute.For<User>();
            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);

            var player = Substitute.For<Player>();
            player.User.Returns(user);
            player.GetTroops(TroopType.Archers).Returns(new Troops(TroopType.Archers, 10, 10));
            player.GetTroops(TroopType.Cavalry).Returns(new Troops(TroopType.Cavalry, 10, 10));
            player.GetTroops(TroopType.Footmen).Returns(new Troops(TroopType.Footmen, 10, 10));

            _context.Users.Add(user);
            _context.Players.Add(player);
            _player = player;
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Succeeds() {
            var handler = new UntrainTroopsCommandHandler(_repository);
            var command = new UntrainTroopsCommand("test@test.com", "0", "1", "2", "3", "4", "5");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.Received().UntrainTroops(TroopType.Archers, 0, 1);
            _player.Received().UntrainTroops(TroopType.Cavalry, 2, 3);
            _player.Received().UntrainTroops(TroopType.Footmen, 4, 5);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Allows_Empty_Troops() {
            var handler = new UntrainTroopsCommandHandler(_repository);
            var command = new UntrainTroopsCommand("test@test.com", "", "", "", "", "", "");

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Alphanumeric_Archers() {
            var handler = new UntrainTroopsCommandHandler(_repository);
            var command = new UntrainTroopsCommand("test@test.com", "A", "2", "2", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Archers");
            result.Errors[0].Message.Should().Be("Archers must be a valid number");
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Alphanumeric_MercenaryArchers() {
            var handler = new UntrainTroopsCommandHandler(_repository);
            var command = new UntrainTroopsCommand("test@test.com", "2", "A", "2", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.MercenaryArchers");
            result.Errors[0].Message.Should().Be("Archer mercenaries must be a valid number");
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Alphanumeric_Cavalry() {
            var handler = new UntrainTroopsCommandHandler(_repository);
            var command = new UntrainTroopsCommand("test@test.com", "2", "2", "A", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Cavalry");
            result.Errors[0].Message.Should().Be("Cavalry must be a valid number");
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Alphanumeric_MercenaryCavalry() {
            var handler = new UntrainTroopsCommandHandler(_repository);
            var command = new UntrainTroopsCommand("test@test.com", "2", "2", "2", "A", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.MercenaryCavalry");
            result.Errors[0].Message.Should().Be("Cavalry mercenaries must be a valid number");
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Alphanumeric_Footmen() {
            var handler = new UntrainTroopsCommandHandler(_repository);
            var command = new UntrainTroopsCommand("test@test.com", "2", "2", "2", "2", "A", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Footmen");
            result.Errors[0].Message.Should().Be("Footmen must be a valid number");
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Alphanumeric_MercenaryFootmen() {
            var handler = new UntrainTroopsCommandHandler(_repository);
            var command = new UntrainTroopsCommand("test@test.com", "2", "2", "2", "2", "2", "A");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.MercenaryFootmen");
            result.Errors[0].Message.Should().Be("Footman mercenaries must be a valid number");
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Too_High_Archers() {
            var handler = new UntrainTroopsCommandHandler(_repository);
            var command = new UntrainTroopsCommand("test@test.com", "12", "2", "2", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Archers");
            result.Errors[0].Message.Should().Be("You don't have that many archers to untrain");
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Too_High_MercenaryArchers() {
            var handler = new UntrainTroopsCommandHandler(_repository);
            var command = new UntrainTroopsCommand("test@test.com", "2", "12", "2", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.MercenaryArchers");
            result.Errors[0].Message.Should().Be("You don't have that many archer mercenaries to untrain");
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Too_High_Cavalry() {
            var handler = new UntrainTroopsCommandHandler(_repository);
            var command = new UntrainTroopsCommand("test@test.com", "2", "2", "12", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Cavalry");
            result.Errors[0].Message.Should().Be("You don't have that many cavalry units to untrain");
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Too_High_MercenaryCavalry() {
            var handler = new UntrainTroopsCommandHandler(_repository);
            var command = new UntrainTroopsCommand("test@test.com", "2", "2", "2", "12", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.MercenaryCavalry");
            result.Errors[0].Message.Should().Be("You don't have that many cavalry mercenaries to untrain");
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Too_High_Footmen() {
            var handler = new UntrainTroopsCommandHandler(_repository);
            var command = new UntrainTroopsCommand("test@test.com", "2", "2", "2", "2", "12", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Footmen");
            result.Errors[0].Message.Should().Be("You don't have that many footmen to untrain");
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Too_High_MercenaryFootmen() {
            var handler = new UntrainTroopsCommandHandler(_repository);
            var command = new UntrainTroopsCommand("test@test.com", "2", "2", "2", "2", "2", "12");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.MercenaryFootmen");
            result.Errors[0].Message.Should().Be("You don't have that many footman mercenaries to untrain");
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Negative_Archers() {
            var handler = new UntrainTroopsCommandHandler(_repository);
            var command = new UntrainTroopsCommand("test@test.com", "-2", "2", "2", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Archers");
            result.Errors[0].Message.Should().Be("Archers must be a valid number");
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Negative_MercenaryArchers() {
            var handler = new UntrainTroopsCommandHandler(_repository);
            var command = new UntrainTroopsCommand("test@test.com", "2", "-2", "2", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.MercenaryArchers");
            result.Errors[0].Message.Should().Be("Archer mercenaries must be a valid number");
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Negative_Cavalry() {
            var handler = new UntrainTroopsCommandHandler(_repository);
            var command = new UntrainTroopsCommand("test@test.com", "2", "2", "-2", "2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Cavalry");
            result.Errors[0].Message.Should().Be("Cavalry must be a valid number");
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Negative_MercenaryCavalry() {
            var handler = new UntrainTroopsCommandHandler(_repository);
            var command = new UntrainTroopsCommand("test@test.com", "2", "2", "2", "-2", "2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.MercenaryCavalry");
            result.Errors[0].Message.Should().Be("Cavalry mercenaries must be a valid number");
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Negative_Footmen() {
            var handler = new UntrainTroopsCommandHandler(_repository);
            var command = new UntrainTroopsCommand("test@test.com", "2", "2", "2", "2", "-2", "2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Footmen");
            result.Errors[0].Message.Should().Be("Footmen must be a valid number");
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
        }

        [TestMethod]
        public void UntrainTroopsCommandHandler_Fails_For_Negative_MercenaryFootmen() {
            var handler = new UntrainTroopsCommandHandler(_repository);
            var command = new UntrainTroopsCommand("test@test.com", "2", "2", "2", "2", "2", "-2");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.MercenaryFootmen");
            result.Errors[0].Message.Should().Be("Footman mercenaries must be a valid number");
            _player.DidNotReceiveWithAnyArgs().UntrainTroops(default, default, default);
        }
    }
}
