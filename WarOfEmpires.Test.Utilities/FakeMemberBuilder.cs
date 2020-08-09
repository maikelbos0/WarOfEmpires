using NSubstitute;
using System;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Test.Utilities {
    public class FakeMemberBuilder : FakePlayerBuilder {
        public Alliance Alliance { get; }

        internal FakeMemberBuilder(FakeWarContext context, Alliance alliance, int id, string email, string displayName, int rank, TitleType title, DateTime? lastOnline, UserStatus status, int attackTurns, int bankTurns, bool canAffordAnything, int stamina) 
            : base(context, id, email, displayName, rank, title, lastOnline, status, attackTurns, bankTurns, canAffordAnything, stamina) {

            Alliance = alliance;

            Player.Alliance.Returns(Alliance);
            Alliance.Members.Add(Player);
        }
    }
}