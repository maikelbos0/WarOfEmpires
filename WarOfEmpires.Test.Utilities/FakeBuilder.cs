using NSubstitute;
using System;
using WarOfEmpires.Domain.Events;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Test.Utilities {
    public class FakeBuilder {
        public FakeWarContext Context { get; }

        public FakeBuilder() {
            Context = new FakeWarContext();
        }

        internal FakeBuilder(FakeWarContext context) {
            Context = context;
        }

        public FakeAllianceBuilder BuildAlliance(int id, string code = "FS", string name = "Føroyskir Samgonga") {
            return new FakeAllianceBuilder(Context, id, code, name);
        }

        public FakePlayerBuilder BuildPlayer(int id, string email = null, string displayName = null, int rank = 0, TitleType title = TitleType.SubChieftain, DateTime? lastOnline = null, UserStatus status = UserStatus.Active, int attackTurns = 20, int bankTurns = 1, bool canAffordAnything = true) {
            return new FakePlayerBuilder(Context, id, email, displayName, rank, title, lastOnline, status, attackTurns, bankTurns, canAffordAnything);
        }

        public FakeBuilder WithPlayer(int id, out Player player, string email = null, string displayName = null, int rank = 0, TitleType title = TitleType.SubChieftain, DateTime? lastOnline = null, UserStatus status = UserStatus.Active, int attackTurns = 20, int bankTurns = 1, bool canAffordAnything = true) {
            player = BuildPlayer(id, email, displayName, rank, title, lastOnline, status, attackTurns, bankTurns, canAffordAnything).Player;

            return this;
        }

        public FakeBuilder WithPlayer(int id, string email = null, string displayName = null, int rank = 0, TitleType title = TitleType.SubChieftain, DateTime? lastOnline = null, UserStatus status = UserStatus.Active, int attackTurns = 20, int bankTurns = 1, bool canAffordAnything = true) {
            BuildPlayer(id, email, displayName, rank, title, lastOnline, status, attackTurns, bankTurns, canAffordAnything);

            return this;
        }

        public FakeBuilder WithScheduledTask(bool isPaused) {
            var task = Substitute.For<ScheduledTask>();

            task.IsPaused.Returns(isPaused);
            Context.ScheduledTasks.Add(task);

            return this;
        }
    }
}