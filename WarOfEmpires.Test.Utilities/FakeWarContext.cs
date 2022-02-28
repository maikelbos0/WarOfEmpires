using WarOfEmpires.Database;
using Microsoft.EntityFrameworkCore;
using Alliances = WarOfEmpires.Domain.Alliances;
using Auditing = WarOfEmpires.Domain.Auditing;
using Events = WarOfEmpires.Domain.Events;
using Game = WarOfEmpires.Domain.Game;
using Players = WarOfEmpires.Domain.Players;
using Security = WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Test.Utilities {

    public sealed class FakeWarContext : IWarContext {
        public DbSet<Game.GameStatus> GameStatus { get; } = new FakeDbSet<Game.GameStatus>();
        public DbSet<Security.User> Users { get; } = new FakeDbSet<Security.User>();
        public DbSet<Auditing.ActionExecution> ActionExecutions { get; } = new FakeDbSet<Auditing.ActionExecution>();
        public DbSet<Alliances.Alliance> Alliances { get; } = new FakeDbSet<Alliances.Alliance>();
        public DbSet<Players.Player> Players { get; } = new FakeDbSet<Players.Player>();
        public DbSet<Events.ScheduledTask> ScheduledTasks { get; } = new FakeDbSet<Events.ScheduledTask>();

        public int CallsToSaveChanges { get; private set; }

        public int SaveChanges() {
            CallsToSaveChanges++;

            return 0;
        }

        public DbSet<TEntity> Set<TEntity>() where TEntity : class {
            return new FakeDbSet<TEntity>();
        }
    }
}
