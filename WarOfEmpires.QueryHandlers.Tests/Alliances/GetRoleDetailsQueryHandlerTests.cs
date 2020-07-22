using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Alliances;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Tests.Alliances {
    [TestClass]
    public sealed class GetRoleDetailsQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly EnumFormatter _formatter = new EnumFormatter();
        private readonly Alliance _alliance;

        public GetRoleDetailsQueryHandlerTests() {
            _alliance = Substitute.For<Alliance>();

            _alliance.Id.Returns(1);
            _alliance.Code.Returns("FS");
            _alliance.Name.Returns("Føroyskir Samgonga");

            var members = new List<Player>() {
                AddPlayer(1, 3, "test1@test.com", "Test display name 1", UserStatus.Active),
                AddPlayer(2, 0, "test2@test.com", "Test display name 2", UserStatus.Inactive),
                AddPlayer(3, 2, "test3@test.com", "Test display name 3", UserStatus.Active),
                AddPlayer(4, 5, "test4@test.com", "Test display name 4", UserStatus.Active)
            };

            _alliance.Members.Returns(members);
            _alliance.Leader.Returns(members.Last());

            var roles = new List<Role>() {
                AddRole(1, "Manager", members[2], members[3]),
                AddRole(2, "Peasant", members[0], members[1])
            };

            _alliance.Roles.Returns(roles);

            _context.Alliances.Add(_alliance);

            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();
            var alliance = Substitute.For<Alliance>();

            user.Id.Returns(99);
            user.Status.Returns(UserStatus.Active);
            user.Email.Returns("wrong@test.com");

            player.User.Returns(user);
            player.Alliance.Returns(alliance);
            player.Id.Returns(99);

            alliance.Roles.Returns(new List<Role>());

            _context.Users.Add(user);
            _context.Players.Add(player);
        }

        public Role AddRole(int id, string name, params Player[] players) {
            var role = Substitute.For<Role>();

            role.Id.Returns(id);
            role.Name.Returns(name);
            role.Players.Returns(players.ToList());

            return role;
        }

        public Player AddPlayer(int id, int rank, string email, string displayName, UserStatus status) {
            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();

            user.Id.Returns(id);
            user.Status.Returns(status);
            user.Email.Returns(email);

            player.User.Returns(user);
            player.Alliance.Returns(_alliance);
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

            return player;
        }

        [TestMethod]
        public void GetRoleDetailsQueryHandler_Returns_Correct_Information() {
            var handler = new GetRoleDetailsQueryHandler(_context, _formatter);
            var query = new GetRoleDetailsQuery("test3@test.com", "2");

            var result = handler.Execute(query);

            result.Id.Should().Be(2);
            result.Name.Should().Be("Peasant");
            result.Players.Should().HaveCount(1);

            result.Players.Should().ContainSingle(p => p.Id == 1);
            result.Players.Single(p => p.Id == 1).DisplayName.Should().Be("Test display name 1");
            result.Players.Single(p => p.Id == 1).Population.Should().Be(49);
            result.Players.Single(p => p.Id == 1).Rank.Should().Be(3);
            result.Players.Single(p => p.Id == 1).Title.Should().Be("Sub chieftain");
        }

        [TestMethod]
        public void GetRoleDetailsQueryHandler_Throws_Exception_For_Role_Of_Different_Alliance() {
            var handler = new GetRoleDetailsQueryHandler(_context, _formatter);
            var query = new GetRoleDetailsQuery("wrong@test.com", "1");

            Action action = () => handler.Execute(query);

            action.Should().Throw<InvalidOperationException>();
        }
    }
}