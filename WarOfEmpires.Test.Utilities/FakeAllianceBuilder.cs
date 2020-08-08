﻿using NSubstitute;
using System;
using System.Collections.Generic;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Test.Utilities {
    public class FakeAllianceBuilder : FakeBuilder {
        public Alliance Alliance { get; }

        internal FakeAllianceBuilder(FakeWarContext context, int id, string code, string name) : base(context) {
            Alliance = Substitute.For<Alliance>();
            Alliance.Id.Returns(id);
            Alliance.Code.Returns(code);
            Alliance.Name.Returns(name);
            Alliance.Members.Returns(new List<Player>());
            Alliance.ChatMessages.Returns(new List<ChatMessage>());
            Alliance.Invites.Returns(new List<Invite>());
            Alliance.Roles.Returns(new List<Role>());
            Context.Alliances.Add(Alliance);
        }

        public FakeMemberBuilder BuildMember(int id, string email = null, string displayName = null, int rank = 0, TitleType title = TitleType.SubChieftain, DateTime? lastOnline = null, UserStatus status = UserStatus.Active, int attackTurns = 20, int bankTurns = 1, bool canAffordAnything = true) {
            return new FakeMemberBuilder(Context, Alliance, id, email, displayName, rank, title, lastOnline, status, attackTurns, bankTurns, canAffordAnything);
        }

        public FakeAllianceBuilder WithMember(int id, out Player member, string email = null, string displayName = null, int rank = 0, TitleType title = TitleType.SubChieftain, DateTime? lastOnline = null, UserStatus status = UserStatus.Active, int attackTurns = 20, int bankTurns = 1, bool canAffordAnything = true) {
            member = BuildMember(id, email, displayName, rank, title, lastOnline, status, attackTurns, bankTurns, canAffordAnything).Player;

            return this;
        }

        public FakeAllianceBuilder WithMember(int id, string email = null, string displayName = null, int rank = 0, TitleType title = TitleType.SubChieftain, DateTime? lastOnline = null, UserStatus status = UserStatus.Active, int attackTurns = 20, int bankTurns = 1, bool canAffordAnything = true) {
            BuildMember(id, email, displayName, rank, title, lastOnline, status, attackTurns, bankTurns, canAffordAnything);

            return this;
        }

        public FakeMemberBuilder BuildLeader(int id, string email = null, string displayName = null, int rank = 0, TitleType title = TitleType.SubChieftain, DateTime? lastOnline = null, int attackTurns = 20, int bankTurns = 1, bool canAffordAnything = true) {
            var builder = BuildMember(id, email, displayName, rank, title, lastOnline, UserStatus.Active, attackTurns, bankTurns, canAffordAnything);

            Alliance.Leader.Returns(builder.Player);

            return builder;
        }

        public FakeAllianceBuilder WithLeader(int id, out Player leader, string email = null, string displayName = null, int rank = 0, TitleType title = TitleType.SubChieftain, DateTime? lastOnline = null, int attackTurns = 20, int bankTurns = 1, bool canAffordAnything = true) {
            leader = BuildLeader(id, email, displayName, rank, title, lastOnline, attackTurns, bankTurns, canAffordAnything).Player;

            return this;
        }

        public FakeAllianceBuilder WithLeader(int id, string email = null, string displayName = null, int rank = 0, TitleType title = TitleType.SubChieftain, DateTime? lastOnline = null, int attackTurns = 20, int bankTurns = 1, bool canAffordAnything = true) {
            BuildLeader(id, email, displayName, rank, title, lastOnline, attackTurns, bankTurns, canAffordAnything);

            return this;
        }

        public FakeAllianceBuilder WithInvite(int id, out Invite invite, Player player, string subject = "Message subject", string body = "Message body", bool isRead = false, DateTime? date = null) {
            invite = Substitute.For<Invite>();

            invite.Id.Returns(id);
            invite.Alliance.Returns(Alliance);
            invite.Subject.Returns(subject);
            invite.Body.Returns(body);
            invite.IsRead.Returns(isRead);
            invite.Date.Returns(date ?? new DateTime(2020, 1, 5));
            invite.Player.Returns(player);

            player.Invites.Add(invite);
            Alliance.Invites.Add(invite);

            return this;
        }

        public FakeAllianceBuilder WithInvite(int id, Player player, string subject = "Message subject", string body = "Message body", bool isRead = false, DateTime? date = null) {
            return WithInvite(id, out _, player, subject, body, isRead, date);
        }

        public FakeAllianceBuilder WithRole(int id, out Role role, string name, params Player[] players) {
            role = Substitute.For<Role>();

            role.Id.Returns(id);
            role.Name.Returns(name);
            role.Players.Returns(players);

            foreach (var player in players) {
                player.AllianceRole.Returns(role);
            }

            Alliance.Roles.Add(role);

            return this;
        }

        public FakeAllianceBuilder WithRole(int id, string name, params Player[] players) {
            return WithRole(id, out _, name, players);
        }

        public FakeAllianceBuilder WithChatMessage(int id, out ChatMessage chatMessage, Player player, DateTime date, string message) {
            chatMessage = Substitute.For<ChatMessage>();

            chatMessage.Id.Returns(id);
            chatMessage.Player.Returns(player);
            chatMessage.Date.Returns(date);
            chatMessage.Message.Returns(message);
            Alliance.ChatMessages.Add(chatMessage);

            return this;
        }

        public FakeAllianceBuilder WithChatMessage(int id, Player player, DateTime date, string message) {
            return WithChatMessage(id, out _, player, date, message);
        }
    }
}