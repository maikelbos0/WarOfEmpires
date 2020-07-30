using NSubstitute;
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
            _context.Alliances.Add(Alliance);
        }

        public override FakePlayerBuilder CreatePlayer(int id, string email = null, string displayName = null, int rank = 0, TitleType title = TitleType.SubChieftain, UserStatus status = UserStatus.Active) {
            var builder = base.CreatePlayer(id, email, displayName, rank, title, status);

            builder.Player.Alliance.Returns(Alliance);
            Alliance.Members.Add(builder.Player);

            return builder;
        }

        public FakePlayerBuilder CreateLeader(int id, string email = null, string displayName = null, int rank = 0, TitleType title = TitleType.SubChieftain) {
            var builder = CreatePlayer(id, email, displayName, rank, title);

            Alliance.Leader.Returns(builder.Player);

            return builder;
        }
    }
}