using NSubstitute;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Test.Utilities {
    public class FakeBuilder {
        protected readonly IWarContext _context;

        public FakeBuilder(IWarContext context) {
            _context = context;
        }

        public AllianceFakeBuilder CreateAlliance(int id, string name, string code) {
            return new AllianceFakeBuilder(_context, id, name, code);
        }

        public PlayerFakeBuilder CreatePlayer(int id, string email, string displayName, UserStatus status = UserStatus.Active) {
            return new PlayerFakeBuilder(_context, id, email, displayName, status);
        }
    }

    public class AllianceFakeBuilder : FakeBuilder {
        public Alliance Alliance { get; }

        protected AllianceFakeBuilder(IWarContext context, Alliance alliance) : base(context) {
            Alliance = alliance;
        }

        public AllianceFakeBuilder(IWarContext context, int id, string name, string code) : base(context) {
            Alliance = Substitute.For<Alliance>();
            Alliance.Id.Returns(id);
            Alliance.Name.Returns(name);
            Alliance.Code.Returns(code);
            Alliance.Members.Returns(new List<Player>());
            _context.Alliances.Add(Alliance);
        }
        
        public PlayerFakeBuilder CreateLeader(int id, string email, string displayName) {
            var builder = new PlayerFakeBuilder(_context, id, email, displayName);

            builder.Player.Alliance.Returns(Alliance);
            Alliance.Members.Add(builder.Player);

            return builder;
        }
    }

    public class PlayerFakeBuilder : FakeBuilder {
        public User User { get; }
        public Player Player { get; }

        public PlayerFakeBuilder(IWarContext context, int id, string email, string displayName, UserStatus status = UserStatus.Active) : base(context) {
            User = Substitute.For<User>();
            User.Id.Returns(id);
            User.Email.Returns(email);
            User.Status.Returns(status);
            _context.Users.Add(User);

            Player = Substitute.For<Player>();
            Player.DisplayName.Returns(displayName);
            _context.Players.Add(Player);
        }
    }
}