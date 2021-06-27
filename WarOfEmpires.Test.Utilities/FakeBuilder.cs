using NSubstitute;
using System;
using System.Linq;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Events;
using WarOfEmpires.Domain.Game;
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

        public FakeBuilder WithAlliance(int id, out Alliance alliance, string code = "FS", string name = "Føroyskir Samgonga") {
            alliance = BuildAlliance(id, code, name).Alliance;

            return this;
        }

        public FakeUserBuilder BuildUser(int id, string email = null, string password = "test", DateTime? lastOnline = null, UserStatus status = UserStatus.Active) {
            return new FakeUserBuilder(Context, id, email, password, lastOnline, status);
        }

        public FakeBuilder WithUser(int id, string email = null, string password = "test", DateTime? lastOnline = null, UserStatus status = UserStatus.Active) {
            BuildUser(id, email, password, lastOnline, status);

            return this;
        }

        public FakePlayerBuilder BuildPlayer(int id, string email = null, string password = "test", string displayName = null, int rank = 0, TitleType title = TitleType.SubChieftain, DateTime? lastOnline = null, UserStatus status = UserStatus.Active, int attackTurns = 20, int bankTurns = 1, bool canAffordAnything = true, int stamina = 100, TimeSpan? grandOverlordTime = null) {
            return new FakePlayerBuilder(Context, id, email, password, displayName, rank, title, lastOnline, status, attackTurns, bankTurns, canAffordAnything, stamina, grandOverlordTime);
        }

        public FakeBuilder WithPlayer(int id, out Player player, string email = null, string password = "test", string displayName = null, int rank = 0, TitleType title = TitleType.SubChieftain, DateTime? lastOnline = null, UserStatus status = UserStatus.Active, int attackTurns = 20, int bankTurns = 1, bool canAffordAnything = true, int stamina = 100, TimeSpan? grandOverlordTime = null) {
            player = BuildPlayer(id, email, password, displayName, rank, title, lastOnline, status, attackTurns, bankTurns, canAffordAnything, stamina, grandOverlordTime).Player;

            return this;
        }

        public FakeBuilder WithPlayer(int id, string email = null, string password = "test", string displayName = null, int rank = 0, TitleType title = TitleType.SubChieftain, DateTime? lastOnline = null, UserStatus status = UserStatus.Active, int attackTurns = 20, int bankTurns = 1, bool canAffordAnything = true, int stamina = 100, TimeSpan? grandOverlordTime = null) {
            BuildPlayer(id, email, password, displayName, rank, title, lastOnline, status, attackTurns, bankTurns, canAffordAnything, stamina, grandOverlordTime);

            return this;
        }

        public FakeBuilder WithScheduledTask(int id, out ScheduledTask scheduledTask, bool isPaused, Type eventType = null, int successCalls = 0) {
            scheduledTask = Substitute.For<ScheduledTask>();

            scheduledTask.Id.Returns(id);
            scheduledTask.IsPaused.Returns(isPaused);
            scheduledTask.EventType.Returns(eventType?.AssemblyQualifiedName);

            if (successCalls > 0) {
                scheduledTask.Execute().Returns(true, Enumerable.Range(0, successCalls - 1).Select(i => true).Append(false).ToArray());
            }

            Context.ScheduledTasks.Add(scheduledTask);

            return this;
        }

        public FakeBuilder WithScheduledTask(int id, bool isPaused, Type eventType = null, int successCalls = 0) {
            return WithScheduledTask(id, out _, isPaused, eventType, successCalls);
        }

        public FakeBuilder WithGameStatus(int id, out GameStatus gameStatus) {
            gameStatus = Substitute.For<GameStatus>();

            gameStatus.Id.Returns(id);

            Context.GameStatus.Add(gameStatus);

            return this;
        }

        public FakeBuilder WithGameStatus(int id) {
            return WithGameStatus(id, out _);
        }
    }
}