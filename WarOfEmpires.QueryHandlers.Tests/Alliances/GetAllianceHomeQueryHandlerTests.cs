﻿using FluentAssertions;
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

namespace WarOfEmpires.QueryHandlers.Tests.Alliances {
    [TestClass]
    public sealed class GetAllianceHomeQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly Alliance _alliance;

        public GetAllianceHomeQueryHandlerTests() {
            _alliance = Substitute.For<Alliance>();

            _alliance.Id.Returns(1);
            _alliance.Code.Returns("FS");
            _alliance.Name.Returns("Føroyskir Samgonga");
            
            var members = new List<Player>() {
                AddPlayer(1, 3, "test1@test.com", "Test display name 1", UserStatus.Active, new DateTime(2020, 1, 5)),
                AddPlayer(2, 0, "test2@test.com", "Test display name 2", UserStatus.Inactive, new DateTime(2020, 1, 6)),
                AddPlayer(3, 2, "test3@test.com", "Test display name 3", UserStatus.Active, new DateTime(2020, 1, 10))
            };

            _alliance.Members.Returns(members);
            _alliance.Leader.Returns(members.Last());

            var roles = new List<Role>() {
                AddRole(4, "Diva", members[0]),
                AddRole(5, "Superstar", members[2])
            };

            _alliance.Roles.Returns(roles);

            var chatMessages = new List<ChatMessage>() {
                CreateChatMessage(members.First(), new DateTime(2020,2,2), "Hidden"),
                CreateChatMessage(members.First(), DateTime.UtcNow.Date, "Displayed"),
                CreateChatMessage(members[1], DateTime.UtcNow.Date.AddDays(-1), "Visible")
            };

            _alliance.ChatMessages.Returns(chatMessages);

            _context.Alliances.Add(_alliance);
        }

        public Role AddRole(int id, string name, params Player[] players) {
            var role = Substitute.For<Role>();

            role.Id.Returns(id);
            role.Name.Returns(name);
            role.Players.Returns(players);

            foreach (var player in players) {
                player.AllianceRole.Returns(role);
            }

            return role;
        }

        public Player AddPlayer(int id, int rank, string email, string displayName, UserStatus status, DateTime lastOnline) {
            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();

            user.Id.Returns(id);
            user.Status.Returns(status);
            user.Email.Returns(email);
            user.LastOnline.Returns(lastOnline);

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

        public ChatMessage CreateChatMessage(Player player, DateTime date, string message) {
            var chatMessage = Substitute.For<ChatMessage>();

            chatMessage.Player.Returns(player);
            chatMessage.Date.Returns(date);
            chatMessage.Message.Returns(message);

            return chatMessage;
        }

        [TestMethod]
        public void GetAllianceHomeQueryHandler_Returns_Correct_Information() {
            var handler = new GetAllianceHomeQueryHandler(_context);
            var query = new GetAllianceHomeQuery("test1@test.com");

            var result = handler.Execute(query);

            result.Id.Should().Be(1);
            result.Code.Should().Be("FS");
            result.Name.Should().Be("Føroyskir Samgonga");
            result.LeaderId.Should().Be(3);
            result.Leader.Should().Be("Test display name 3");
            result.Members.Should().HaveCount(2);
            result.Members.First().Id.Should().Be(3);
            result.Members.First().LastOnline.Should().Be(new DateTime(2020, 1, 10));
            result.Members.First().Rank.Should().Be(2);
            result.Members.First().DisplayName.Should().Be("Test display name 3");
            result.Members.First().Role.Should().Be("Superstar");
        }

        [TestMethod]
        public void GetAllianceHomeQueryHandler_Returns_Only_Recent_ChatMessages() {
            var handler = new GetAllianceHomeQueryHandler(_context);
            var query = new GetAllianceHomeQuery("test1@test.com");

            var result = handler.Execute(query);

            result.ChatMessages.Should().HaveCount(2);
            result.ChatMessages.First().PlayerId.Should().Be(1);
            result.ChatMessages.First().Player.Should().Be("Test display name 1");
            result.ChatMessages.First().Message.Should().Be("Displayed");
            result.ChatMessages.Last().PlayerId.Should().Be(null);
            result.ChatMessages.Last().Player.Should().Be("Test display name 2");
            result.ChatMessages.Last().Message.Should().Be("Visible");
        }
    }
}