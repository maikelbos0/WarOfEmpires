using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Players;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Tests.Players {
    [TestClass]
    public sealed class GetPlayersQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly EnumFormatter _formatter = new EnumFormatter();

        public void AddPlayer(int id, int rank, string email, string displayName, UserStatus status) {
            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();

            user.Id.Returns(id);
            user.Status.Returns(status);
            user.Email.Returns(email);

            player.User.Returns(user);
            player.Id.Returns(id);
            player.Rank.Returns(rank);
            player.Title.Returns(TitleType.SubChieftain);
            player.DisplayName.Returns(displayName);
            player.Peasants.Returns(5);
            player.Workers.Returns(new List<Workers>() {
                new Workers(WorkerType.Farmers, 1),
                new Workers(WorkerType.WoodWorkers, 2),
                new Workers(WorkerType.StoneMasons, 3),
                new Workers(WorkerType.OreMiners, 4),
                new Workers(WorkerType.SiegeEngineers, 6)
            });
            player.Troops.Returns(new List<Troops>() {
                new Troops(TroopType.Archers, 15, 5),
                new Troops(TroopType.Cavalry, 3, 1),
                new Troops(TroopType.Footmen, 3, 1)
            });
            
            _context.Users.Add(user);
            _context.Players.Add(player);
        }

        [TestMethod]
        public void GetPlayersQueryHandler_Returns_All_Active_Players() {
            for (var i = 0; i < 9; i++) {
                AddPlayer(i + 1, i + 1, $"test{i}@test.com", $"Test {i}", (UserStatus)(i % 3 + 1));
            }

            var handler = new GetPlayersQueryHandler(_context, _formatter);
            var query = new GetPlayersQuery(null);

            var result = handler.Execute(query);

            result.Should().HaveCount(3);
        }

        [TestMethod]
        public void GetPlayersQueryHandler_Returns_Correct_Information() {
            var handler = new GetPlayersQueryHandler(_context, _formatter);
            var query = new GetPlayersQuery(null);

            AddPlayer(1, 5, "test@test.com", "Test display name", UserStatus.Active);

            var result = handler.Execute(query);

            result.Should().HaveCount(1);
            result.Single().Id.Should().Be(1);
            result.Single().Rank.Should().Be(5);
            result.Single().Title.Should().Be("Sub chieftain");
            result.Single().DisplayName.Should().Be("Test display name");
            result.Single().Population.Should().Be(49);
        }

        [TestMethod]
        public void GetPlayersQueryHandler_Searches() {
            var handler = new GetPlayersQueryHandler(_context, _formatter);
            var query = new GetPlayersQuery("Test");

            AddPlayer(1, 2, "test@test.com", "Test display name", UserStatus.Active);
            AddPlayer(2, 1, "test@test.com", "Somebody else", UserStatus.Active);

            var result = handler.Execute(query);

            result.Should().HaveCount(1);
            result.Single().DisplayName.Should().Be("Test display name");
        }
    }
}