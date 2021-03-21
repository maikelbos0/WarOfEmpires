using WarOfEmpires.Database;
using Alliances = WarOfEmpires.Domain.Alliances;
using Auditing = WarOfEmpires.Domain.Auditing;
using Events = WarOfEmpires.Domain.Events;
using Players = WarOfEmpires.Domain.Players;
using Security = WarOfEmpires.Domain.Security;
using Microsoft.EntityFrameworkCore;

namespace WarOfEmpires.Test.Utilities {
    public sealed class FakeWarContext : DbContext, IWarContext {
        public int CallsToSaveChanges { get; private set; }
        public DbSet<Security.User> Users { get; set; }
        public DbSet<Auditing.CommandExecution> CommandExecutions { get; set; }
        public DbSet<Alliances.Alliance> Alliances { get; set; }
        public DbSet<Auditing.QueryExecution> QueryExecutions { get; set; }
        public DbSet<Players.Player> Players { get; set; }
        public DbSet<Events.ScheduledTask> ScheduledTasks { get; set; }

        public FakeWarContext() : base(new DbContextOptionsBuilder<FakeWarContext>().UseInMemoryDatabase("WarOfEmpires").Options) {
        }

        public override int SaveChanges() {
            CallsToSaveChanges++;

            return base.SaveChanges();
        }
    }
}