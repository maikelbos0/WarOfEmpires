using NSubstitute;
using System;
using System.Collections.Generic;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Test.Utilities {
    public class FakeAllianceBuilder : FakeBuilder {
        public Alliance Alliance { get; }

        protected FakeAllianceBuilder(IWarContext context, Alliance alliance) : base(context) {
            Alliance = alliance;
        }

        internal FakeAllianceBuilder(IWarContext context, int id, string code, string name) : base(context) {
            Alliance = Substitute.For<Alliance>();
            Alliance.Id.Returns(id);
            Alliance.Code.Returns(code);
            Alliance.Name.Returns(name);
            Alliance.Members.Returns(new List<Player>());
            Alliance.ChatMessages.Returns(new List<ChatMessage>());
            Alliance.Invites.Returns(new List<Invite>());
            Context.Alliances.Add(Alliance);
        }

        public FakePlayerBuilder CreateMember(int id, string email = null, string displayName = null, int rank = 0, TitleType title = TitleType.SubChieftain, DateTime? lastOnline = null, UserStatus status = UserStatus.Active) {
            var builder = CreatePlayer(id, email, displayName, rank, title, lastOnline, status);

            builder.Player.Alliance.Returns(Alliance);
            Alliance.Members.Add(builder.Player);

            return builder;
        }

        public FakePlayerBuilder CreateLeader(int id, string email = null, string displayName = null, int rank = 0, DateTime? lastOnline = null, TitleType title = TitleType.SubChieftain) {
            var builder = CreateMember(id, email, displayName, rank, title, lastOnline);

            Alliance.Leader.Returns(builder.Player);

            return builder;
        }

        public FakeAllianceBuilder AddInvite(int id, Player player, string subject = "Message subject", string body = "Message body", bool isRead = false, DateTime? date = null) {
            var invite = Substitute.For<Invite>();

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
    }
}