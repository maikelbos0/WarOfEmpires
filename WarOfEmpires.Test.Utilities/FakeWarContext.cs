using WarOfEmpires.Database;
using Microsoft.EntityFrameworkCore;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Auditing;
using WarOfEmpires.Domain.Events;
using WarOfEmpires.Domain.Game;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Test.Utilities {

    public sealed class FakeWarContext : IWarContext {
        public DbSet<GameStatus> GameStatus { get; } = new FakeDbSet<GameStatus>();
        public DbSet<User> Users { get; } = new FakeDbSet<User>();
        public DbSet<ActionExecution> ActionExecutions { get; } = new FakeDbSet<ActionExecution>();
        public DbSet<Alliance> Alliances { get; } = new FakeDbSet<Alliance>();
        public DbSet<Player> Players { get; } = new FakeDbSet<Player>();
        public DbSet<ScheduledTask> ScheduledTasks { get; } = new FakeDbSet<ScheduledTask>();

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
