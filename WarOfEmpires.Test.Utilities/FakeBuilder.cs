using System;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Test.Utilities {
    public class FakeBuilder {
        protected readonly IWarContext _context;

        public FakeBuilder(IWarContext context) {
            _context = context;
        }

        public FakeAllianceBuilder CreateAlliance(int id, string code = "FS", string name = "Føroyskir Samgonga") {
            return new FakeAllianceBuilder(_context, id, code, name);
        }

        public virtual FakePlayerBuilder CreatePlayer(int id, string email = null, string displayName = null, int rank = 0, TitleType title = TitleType.SubChieftain, DateTime? lastOnline = null, UserStatus status = UserStatus.Active) {
            return new FakePlayerBuilder(_context, id, email, displayName, rank, title, lastOnline, status);
        }
    }
}