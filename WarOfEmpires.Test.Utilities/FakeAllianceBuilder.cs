using NSubstitute;
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
            Alliance.NonAggressionPacts.Returns(new List<NonAggressionPact>());
            Alliance.SentNonAggressionPactRequests.Returns(new List<NonAggressionPactRequest>());
            Alliance.ReceivedNonAggressionPactRequests.Returns(new List<NonAggressionPactRequest>());
            Alliance.Wars.Returns(new List<War>());
            Context.Alliances.Add(Alliance);
        }

        public FakeMemberBuilder BuildMember(int id, string email = null, string password = "test", string displayName = null, int rank = 0, TitleType title = TitleType.SubChieftain, DateTime? lastOnline = null, UserStatus status = UserStatus.Active, int attackTurns = 20, int bankTurns = 1, bool canAffordAnything = true, int stamina = 100, TimeSpan? grandOverlordTime = null, DateTime? creationDate = null) {
            return new FakeMemberBuilder(Context, Alliance, id, email, password, displayName, rank, title, lastOnline, status, attackTurns, bankTurns, canAffordAnything, stamina, grandOverlordTime, creationDate ?? DateTime.MinValue);
        }

        public FakeAllianceBuilder WithMember(int id, out Player member, string email = null, string password = "test", string displayName = null, int rank = 0, TitleType title = TitleType.SubChieftain, DateTime? lastOnline = null, UserStatus status = UserStatus.Active, int attackTurns = 20, int bankTurns = 1, bool canAffordAnything = true, int stamina = 100, TimeSpan? grandOverlordTime = null, DateTime? creationDate = null) {
            member = BuildMember(id, email, password, displayName, rank, title, lastOnline, status, attackTurns, bankTurns, canAffordAnything, stamina, grandOverlordTime, creationDate).Player;

            return this;
        }

        public FakeAllianceBuilder WithMember(int id, string email = null, string password = "test", string displayName = null, int rank = 0, TitleType title = TitleType.SubChieftain, DateTime? lastOnline = null, UserStatus status = UserStatus.Active, int attackTurns = 20, int bankTurns = 1, bool canAffordAnything = true, int stamina = 100, TimeSpan? grandOverlordTime = null, DateTime? creationDate = null) {
            BuildMember(id, email, password, displayName, rank, title, lastOnline, status, attackTurns, bankTurns, canAffordAnything, stamina, grandOverlordTime, creationDate);

            return this;
        }

        public FakeMemberBuilder BuildLeader(int id, string email = null, string password = "test", string displayName = null, int rank = 0, TitleType title = TitleType.SubChieftain, DateTime? lastOnline = null, int attackTurns = 20, UserStatus status = UserStatus.Active, int bankTurns = 1, bool canAffordAnything = true, int stamina = 100, TimeSpan? grandOverlordTime = null, DateTime? creationDate = null) {
            var builder = BuildMember(id, email, password, displayName, rank, title, lastOnline, status, attackTurns, bankTurns, canAffordAnything, stamina, grandOverlordTime, creationDate);

            Alliance.Leader.Returns(builder.Player);

            return builder;
        }

        public FakeAllianceBuilder WithLeader(int id, out Player leader, string email = null, string password = "test", string displayName = null, int rank = 0, TitleType title = TitleType.SubChieftain, DateTime? lastOnline = null, UserStatus status = UserStatus.Active, int attackTurns = 20, int bankTurns = 1, bool canAffordAnything = true, int stamina = 100, TimeSpan? grandOverlordTime = null, DateTime? creationDate = null) {
            leader = BuildLeader(id, email, password, displayName, rank, title, lastOnline, attackTurns, status, bankTurns, canAffordAnything, stamina, grandOverlordTime, creationDate).Player;

            return this;
        }

        public FakeAllianceBuilder WithLeader(int id, string email = null, string password = "test", string displayName = null, int rank = 0, TitleType title = TitleType.SubChieftain, DateTime? lastOnline = null, UserStatus status = UserStatus.Active, int attackTurns = 20, int bankTurns = 1, bool canAffordAnything = true, int stamina = 100, TimeSpan? grandOverlordTime = null, DateTime? creationDate = null) {
            BuildLeader(id, email, password, displayName, rank, title, lastOnline, attackTurns, status, bankTurns, canAffordAnything, stamina, grandOverlordTime, creationDate);

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
            role.CanInvite.Returns(true);
            role.CanManageRoles.Returns(true);
            role.CanDeleteChatMessages.Returns(true);
            role.CanKickMembers.Returns(true);
            role.CanManageNonAggressionPacts.Returns(true);
            role.CanManageWars.Returns(true);
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

        public FakeAllianceBuilder WithNonAggressionPact(int id, out NonAggressionPact pact, Alliance alliance) {
            pact = Substitute.For<NonAggressionPact>();

            pact.Id.Returns(id);
            pact.Alliances.Returns(new List<Alliance>() { Alliance, alliance });
            Alliance.NonAggressionPacts.Add(pact);
            alliance.NonAggressionPacts.Add(pact);

            return this;
        }

        public FakeAllianceBuilder WithNonAggressionPact(int id, Alliance alliance) {
            return WithNonAggressionPact(id, out _, alliance);
        }

        public FakeAllianceBuilder WithNonAggressionPactRequestTo(int id, out NonAggressionPactRequest request, Alliance recipient) {
            request = Substitute.For<NonAggressionPactRequest>();

            request.Id.Returns(id);
            request.Sender.Returns(Alliance);
            request.Recipient.Returns(recipient);
            Alliance.SentNonAggressionPactRequests.Add(request);
            recipient.ReceivedNonAggressionPactRequests.Add(request);

            return this;
        }

        public FakeAllianceBuilder WithNonAggressionPactRequestTo(int id, Alliance recipient) {
            return WithNonAggressionPactRequestTo(id, out _, recipient);
        }

        public FakeAllianceBuilder WithNonAggressionPactRequestFrom(int id, out NonAggressionPactRequest request, Alliance sender) {
            request = Substitute.For<NonAggressionPactRequest>();

            request.Id.Returns(id);
            request.Recipient.Returns(Alliance);
            request.Sender.Returns(sender);
            Alliance.ReceivedNonAggressionPactRequests.Add(request);
            sender.SentNonAggressionPactRequests.Add(request);

            return this;
        }

        public FakeAllianceBuilder WithNonAggressionPactRequestFrom(int id, Alliance sender) {
            return WithNonAggressionPactRequestFrom(id, out _, sender);
        }

        public FakeAllianceBuilder WithWar(int id, out War war, Alliance alliance, bool peaceDeclared = false, bool peaceOffered = false) {
            war = Substitute.For<War>();

            war.Id.Returns(id);
            war.Alliances.Returns(new List<Alliance>() { Alliance, alliance });
            war.PeaceDeclarations.Returns(new List<Alliance>());
            Alliance.Wars.Add(war);
            alliance.Wars.Add(war);

            if (peaceDeclared) {
                war.PeaceDeclarations.Add(Alliance);
            }

            if (peaceOffered) {
                war.PeaceDeclarations.Add(alliance);
            }

            return this;
        }

        public FakeAllianceBuilder WithWar(int id, Alliance alliance, bool peaceDeclared = false, bool peaceOffered = false) {
            return WithWar(id, out _, alliance, peaceDeclared, peaceOffered);
        }
    }
}
