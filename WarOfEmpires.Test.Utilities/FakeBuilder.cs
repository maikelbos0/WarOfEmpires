using NSubstitute;
using System;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Events;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Test.Utilities {
    public class FakeBuilder {
        public IWarContext Context { get; }

        public FakeBuilder() {
            Context = new FakeWarContext();
        }

        internal FakeBuilder(IWarContext context) {
            Context = context;
        }

        public FakeAllianceBuilder BuildAlliance(int id, string code = "FS", string name = "Føroyskir Samgonga") {
            return new FakeAllianceBuilder(Context, id, code, name);
        }

        public FakePlayerBuilder BuildPlayer(int id, string email = null, string displayName = null, int rank = 0, TitleType title = TitleType.SubChieftain, DateTime? lastOnline = null, UserStatus status = UserStatus.Active) {
            return new FakePlayerBuilder(Context, id, email, displayName, rank, title, lastOnline, status);
        }

        public FakeBuilder WithScheduledTask(bool isPaused) {
            var task = Substitute.For<ScheduledTask>();

            task.IsPaused.Returns(isPaused);
            Context.ScheduledTasks.Add(task);

            return this;
        }
    }
}